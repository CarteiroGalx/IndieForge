import axios from "axios";
import { useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import { useNavigate } from "react-router-dom";
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
  const [modalShow, setModalShow] = useState(false);
  const [project, setProject] = useState<ProjectDetailsData | null>(null);
  const [loadingProject, setLoadingProject] = useState(true);
  const [contributionValue, setContributionValue] = useState<number>(0);
  const navigate = useNavigate();

  const [errorModal, setErrorModal] = useState(false);
  const [successModal, setSuccessModal] = useState(false);
  const [errorMessage, setErrorMessage] = useState("");
  const [sucessMessage, setSucessMessage] = useState("");

  useEffect(() => {
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
  

  useEffect(() => {
    axios
      .get("http://localhost:5259/api/check-auth", {
        headers: {
          Authorization: `Bearer ${localStorage.getItem("token")}`,
        },
      })
      .catch(() => {
        localStorage.removeItem("token");
        navigate("/");
      });
  });

  const formatCurrency = (value: number) => {
    return new Intl.NumberFormat("pt-BR", {
      style: "currency",
      currency: "BRL",
    }).format(value);
  };

  const handleContribution = (event: React.SubmitEvent<HTMLFormElement>) => {
    event.preventDefault();
    if (contributionValue <= 0) {
      alert("Por favor, insira um valor válido para a contribuição.");
      return;
    }

    axios
      .post(
        `http://localhost:5259/api/projects/${projectId}/contributions`,
        { valor: contributionValue },
        {
          headers: {
            Authorization: `Bearer ${localStorage.getItem("token")}`,
          },
        },
      )
      .then((response) => {
        setModalShow(false);
        setSuccessModal(true);
        setContributionValue(0);
        setSucessMessage(response.data.message);
        return axios.get(`http://localhost:5259/api/projects/${projectId}`);
      })
      .then((response) => {
        setProject(response.data);
        setSuccessModal(false);
      })
      .catch((err) => {
        setModalShow(false);
        setErrorMessage(
          err.response?.data?.message ||
            "Ocorreu um erro ao enviar a contribuição.",
        );
        setErrorModal(true);
        console.error("Erro ao enviar contribuição:", err);
      });
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

  if (!project) {
    return (
      <S.Page>
        <main className="container">
          <Link to="/home" className="back-link">
            Voltar
          </Link>
          <section className="empty-state">
            <h1>Projeto não encontrado</h1>
            <p>{errorMessage || "Nao encontramos dados para este projeto."}</p>
          </section>
        </main>
      </S.Page>
    );
  }

  const progress = Math.min(project.porcentagem, 100);

  return (
    <S.Page>
      {modalShow && (
        <div className="modal" tabIndex={-1} style={{ display: "block" }}>
          <div className="modal-dialog-centered modal-dialog">
            <div className="modal-content border border-2 border-warning bg-dark text-white">
              <div className="modal-body">
                <h3>Insira valor da contribuição</h3>
                <input
                  type="number"
                  value={contributionValue}
                  onChange={(e) => setContributionValue(Number(e.target.value))}
                  className="form-control bg-black border-warning text-white"
                />
              </div>
              <div className="modal-footer border-0">
                <button
                  type="button"
                  className="btn btn-secondary"
                  data-bs-dismiss="modal"
                  onClick={() => setModalShow(false)}
                >
                  Cancelar
                </button>
                <button
                  type="button"
                  className="btn btn-warning"
                  onClick={handleContribution}
                >
                  Confirmar
                </button>
              </div>
            </div>
          </div>
        </div>
      )}
      {(successModal || errorModal) && (
        <div className="modal" tabIndex={-1} style={{ display: "block" }}>
          <div className="modal-dialog-centered modal-dialog">
            <div
              className={`modal-content border border-2 ${successModal ? "border-success" : "border-danger"} bg-dark text-white`}
            >
              <div className="modal-body">
                <h3>{successModal ? "Sucesso!" : "Erro!"}</h3>
                <p>{successModal ? sucessMessage : errorMessage}</p>
              </div>
              <div className="modal-footer border-0">
                <button
                  type="button"
                  className={`btn ${successModal ? "btn-success" : "btn-danger"}`}
                  onClick={() => {
                    setSuccessModal(false);
                    setErrorModal(false);
                  }}
                >
                  Fechar
                </button>
              </div>
            </div>
          </div>
        </div>
      )}
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
            <button
              className="col-12 btn btn-warning mt-1"
              onClick={() => setModalShow(true)}
            >
              <strong>Contribuir</strong>
            </button>
          </div>
        </section>

        <section className="progress-section">
          <div className="progress-heading">
            <span>Progresso da campanha</span>
            <strong>{project.porcentagem.toFixed(2)}%</strong>
          </div>
          <div className="progress-track">
            <div className="progress-fill" style={{ width: `${progress}%` }} />
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
