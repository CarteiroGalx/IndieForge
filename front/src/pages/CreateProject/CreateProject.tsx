import axios from "axios";
import React, { useState } from "react";
import { Link, useNavigate } from "react-router-dom";

interface CreateProjectResponse {
  id: string;
  nome: string;
  descricao: string;
  metaFinanceira: number;
  status: number;
}

export default function CreateProject() {
  const [nome, setNome] = useState("");
  const [descricao, setDescricao] = useState("");
  const [metaFinanceira, setMetaFinanceira] = useState("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  const handleCreateProject = async (
    event: React.FormEvent<HTMLFormElement>,
  ) => {
    event.preventDefault();
    setLoading(true);
    setError(null);

    try {
      const response = await axios.post<CreateProjectResponse>(
        "http://localhost:5259/api/projects/",
        {
          nome,
          descricao,
          metaFinanceira: Number(metaFinanceira),
        },
        {
          headers: {
            Authorization: `Bearer ${localStorage.getItem("token")}`,
          },
        },
      );

      navigate(`/projects/${response.data.id}`);
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    } catch (err: any) {
      const errors = err.response?.data?.errors;

      if (errors?.Nome || errors?.Descricao || errors?.MetaFinanceira) {
        setError(
          errors?.Nome?.[0] ||
            errors?.Descricao?.[0] ||
            errors?.MetaFinanceira?.[0],
        );
      } else if (err.response?.status === 400) {
        setError("Confira os dados do projeto e tente novamente.");
      } else if (err.response?.status === 401) {
        setError("Voce precisa estar logado para criar um projeto.");
      } else {
        setError("Nao foi possivel criar o projeto. Tente novamente.");
      }

      console.error("Create project error:", err.response?.data || err);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div
      className="d-flex align-items-center justify-content-center min-vh-100 py-4"
      style={{ backgroundColor: "#0a0a0a" }}
    >
      <div
        className="card shadow-lg my-3"
        style={{
          width: "520px",
          border: "0",
          borderRadius: "1rem",
          backgroundColor: "#111111",
        }}
      >
        {loading && (
          <div
            className="rounded h-100 w-100 text-white d-flex"
            style={{
              backgroundColor: "#111111",
              position: "absolute",
              top: 0,
              left: 0,
              zIndex: 10,
              justifyContent: "center",
              alignItems: "center",
            }}
          >
            <h2>Carregando...</h2>
          </div>
        )}

        <div className="card-body p-4">
          <div className="text-center mb-4">
            <h1 className="h3 fw-bold" style={{ color: "#ffaa00" }}>
              Criar projeto
            </h1>
            <p className="text-white-50 mb-0">
              Preencha as informacoes iniciais da sua campanha.
            </p>
          </div>

          <form onSubmit={handleCreateProject}>
            <div className="mb-3">
              <label htmlFor="nome" className="form-label text-white">
                Nome do projeto
              </label>
              <input
                type="text"
                id="nome"
                className="form-control bg-dark text-white border-secondary"
                value={nome}
                onChange={(event) => setNome(event.target.value)}
                placeholder="Ex: Projeto Sigma"
                required
              />
            </div>

            <div className="mb-3">
              <label htmlFor="descricao" className="form-label text-white">
                Descricao
              </label>
              <textarea
                id="descricao"
                className="form-control bg-dark text-white border-secondary"
                value={descricao}
                onChange={(event) => setDescricao(event.target.value)}
                placeholder="Conte um pouco sobre o projeto"
                rows={5}
                required
              />
            </div>

            <div className="mb-4">
              <label htmlFor="metaFinanceira" className="form-label text-white">
                Meta financeira
              </label>
              <input
                type="number"
                id="metaFinanceira"
                className="form-control bg-dark text-white border-secondary"
                value={metaFinanceira}
                onChange={(event) => setMetaFinanceira(event.target.value)}
                placeholder="18000"
                min="1"
                step="0.01"
                required
              />
            </div>

            <div className="fs-6 mb-3 text-center">
              {error && <p className="text-danger mb-0">{error}</p>}
            </div>

            <div className="d-grid gap-2 mb-3">
              <button
                type="submit"
                className="btn btn-warning btn-lg text-uppercase fw-semibold"
                disabled={loading}
              >
                Criar projeto
              </button>
            </div>

            <div className="text-center text-white-50 mb-0">
              <small>
                Quer voltar? <Link to="/home">Ir para a Home</Link>
              </small>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
}
