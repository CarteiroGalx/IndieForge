import axios from "axios";
import { useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import * as S from "./ProjectDetails.styles";

interface Contribution {
  valor: number;
  dataCriacao: string;
  nomeApoiador: string;
}

interface ProjectDetailsData {
  nome: string;
  descricao: string;
  metaFinanceira: number;
  totalContribuicoes: number;
  totalArrecadado: number;
  status: number;
  dataCriacao: string;
  contribuicoes: Contribution[];
  porcentagem: number;
}

const statusLabels: Record<number, string> = {
  0: "Em andamento",
  1: "Finalizado",
  2: "Cancelado",
};

export default function ProjectDetails() {
  const { projectId } = useParams();
  const [project, setProject] = useState<ProjectDetailsData | null>(null);
  const [loadingProject, setLoadingProject] = useState(true);
  const [errorMessage, setErrorMessage] = useState("");

  useEffect(() => {
    setLoadingProject(true);
    setErrorMessage("");

    axios
      .get(`http://localhost:5259/api/projects/${projectId}`)
      .then((response) => {
        setProject(response.data);
      })
      .catch((err) => {
        console.log("Problema: " + err);
        setErrorMessage("Nao foi possivel carregar os detalhes do projeto.");
      })
      .finally(() => {
        setLoadingProject(false);
      });
  }, [projectId]);

  const formatCurrency = (value: number) => {
    return new Intl.NumberFormat("pt-BR", {
      style: "currency",
      currency: "BRL",
    }).format(value);
  };

  const formatDate = (date: string) => {
    return new Date(date).toLocaleDateString("pt-BR");
  };

  if (loadingProject) {
    return (
      <S.Page>
        <main className="container">
          <div className="text-center py-5">
            <div className="spinner-border text-warning mb-3" role="status" />
            <p className="text-white-50 mb-0">Carregando detalhes...</p>
          </div>
        </main>
      </S.Page>
    );
  }

  if (errorMessage || !project) {
    return (
      <S.Page>
        <main className="container">
          <Link to="/home" className="back-link">
            Voltar
          </Link>
          <section className="empty-state">
            <h1>Projeto nao encontrado</h1>
            <p>{errorMessage || "Nao encontramos dados para este projeto."}</p>
          </section>
        </main>
      </S.Page>
    );
  }

  const progress = Math.min(project.porcentagem, 100);

  return (
    <S.Page>
      <main className="container">
        <Link to="/home" className="back-link">
          Voltar
        </Link>

        <section className="project-hero">
          <div>
            <span className="status-pill">
              {statusLabels[project.status] || "Status indefinido"}
            </span>
            <h1>{project.nome}</h1>
            <p>{project.descricao}</p>
          </div>

          <div>
            <aside className="summary-card">
              <span>Arrecadado</span>
              <strong>{formatCurrency(project.totalArrecadado)}</strong>
              <small>de {formatCurrency(project.metaFinanceira)}</small>
            </aside>
            <button className="col-12 btn btn-warning mt-1"><strong>Contribuir</strong></button> 
          </div>
        </section>

        <section className="progress-section">
          <div className="progress-heading">
            <span>Progresso da campanha</span>
            <strong>{project.porcentagem.toFixed(2)}%</strong>
          </div>
          <div className="progress-track">
            <div
              className="progress-fill"
              style={{ width: `${progress}%` }}
            />
          </div>
        </section>

        <section className="stats-grid">
          <article>
            <span>Meta financeira</span>
            <strong>{formatCurrency(project.metaFinanceira)}</strong>
          </article>
          <article>
            <span>Total arrecadado</span>
            <strong>{formatCurrency(project.totalArrecadado)}</strong>
          </article>
          <article>
            <span>Contribuicoes</span>
            <strong>{project.totalContribuicoes}</strong>
          </article>
          <article>
            <span>Criado em</span>
            <strong>{formatDate(project.dataCriacao)}</strong>
          </article>
        </section>

        <section className="contributions-section">
          <div className="section-heading">
            <h2>Contribuicoes recentes</h2>
            <span>{project.contribuicoes.length}</span>
          </div>

          {project.contribuicoes.length === 0 ? (
            <div className="empty-state">
              <p>Este projeto ainda nao recebeu contribuicoes.</p>
            </div>
          ) : (
            <div className="contributions-list">
              {project.contribuicoes.map((contribution) => (
                <article
                  className="contribution-card"
                  key={`${contribution.nomeApoiador}-${contribution.dataCriacao}`}
                >
                  <div>
                    <strong>{contribution.nomeApoiador}</strong>
                    <span>{formatDate(contribution.dataCriacao)}</span>
                  </div>
                  <strong>{formatCurrency(contribution.valor)}</strong>
                </article>
              ))}
            </div>
          )}
        </section>
      </main>
    </S.Page>
  );
}
