import axios from "axios";
import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import * as S from "../Home/Home.styles";

interface Project {
  id: string;
  nome: string;
  descricao: string;
  meta: number;
  arrecadado: number;
  percentage: number;
  dataCriacao: string;
  criadorNome: string;
}

interface UserInfo {
  userEmail: string;
  userName: string;
  userRole: string;
}

export default function Home() {
  const [projects, setProjects] = useState<Project[]>([]);
  const [userInfo, setUserInfo] = useState<UserInfo | null>(null);
  const [projectNameSearch, setProjecNameSearch] = useState("");
  const [loadingProjects, setLoadingProjects] = useState(true);

  useEffect(() => {
    axios
      .get("http://localhost:5259/api/projects")
      .then((response) => {
        setProjects(response.data);
      })
      .catch((error) => {
        console.error("Error fetching projects:", error);
      })
      .finally(() => {
        setLoadingProjects(false);
      });
  }, []);

  const getInitials = (name: string) => {
    return name
      .split(" ")
      .filter(Boolean)
      .slice(0, 2)
      .map((part) => part[0])
      .join("")
      .toUpperCase();
  };

  const Logout = () => {
    localStorage.removeItem("token");
  };

  useEffect(() => {
    axios
      .get("http://localhost:5259/api/check-auth", {
        headers: {
          Authorization: `Bearer ${localStorage.getItem("token")}`,
        },
      })
      .then((response) => {
        setUserInfo(response.data);
      })
      .catch((error) => {
        setUserInfo(null);
        localStorage.removeItem("token");
      });
  }, []);

  const formatCurrency = (value: number) => {
    return new Intl.NumberFormat("pt-BR", {
      style: "currency",
      currency: "BRL",
    }).format(value);
  };

  const getProjectsByName = async (
    event: React.SubmitEvent<HTMLFormElement>,
  ) => {
    event.preventDefault();
    setLoadingProjects(true);
    axios
      .get("http://localhost:5259/api/projects?" + "name=" + projectNameSearch)
      .then((response) => {
        setProjects(response.data);
        console.log(response.data);
      })
      .catch((err) => {
        console.log("Deu errado! Erro: " + err);
      })
      .finally(() => {
        setLoadingProjects(false);
      });
  };

  return (
    <S.Div className="min-vh-100 text-white">
      <header className="border-bottom bg-dark position-fixed min-vw-100 z-1">
        <div className="d-flex container py-2 align-items-center justify-content-between">
          <div className="g-2 col-9">
            <form onSubmit={getProjectsByName}>
              <input
                type="text"
                placeholder="Pesquisar"
                value={projectNameSearch}
                onChange={(e) => setProjecNameSearch(e.target.value)}
                id="search-input"
                className="form-control bg-dark-subtle border-0"
              ></input>
            </form>
          </div>
          {userInfo ? (
            <div className="dropdown">
              <button
                className="btn rounded-circle btn-secondary"
                id="icon-profile"
                type="button"
                data-bs-toggle="dropdown"
                aria-expanded="false"
              >
                <div>{getInitials(userInfo.userName)}</div>
              </button>
              <ul className="dropdown-menu dropdown-menu-dark">
                <div className="px-3">
                  <h6 className="mb-0">{userInfo.userName}</h6>
                  <p className="text-white-50 max-width mb-0">
                    {userInfo.userEmail}
                  </p>
                  <strong className="badge text-bg-warning user-select-none">
                    {userInfo.userRole}
                  </strong>
                </div>
                <li>
                  <hr className="dropdown-divider bg-white"></hr>
                </li>
                  <li>
                    <Link to="/profile" className="dropdown-item"><i className="bi bi-file-person"></i> Ver perfil</Link>
                  </li>
                  <li>
                    <Link to="/" className="dropdown-item" onClick={Logout}>
                      <i className="bi bi-door-open-fill"></i> Logout
                    </Link>
                  </li>
                {userInfo.userRole === "Admin" && (
                    <li>
                      <Link to="/admin-center" className="dropdown-item"><i className="bi bi-shield-shaded"></i> Admin Center</Link>
                    </li>
                )}
              </ul>
            </div>
          ) : (
            <div className="d-flex gap-2 col-auto">
              <button className="btn btn-warning">
                <Link className="text-white text-decoration-none" to="/">
                  Entrar
                </Link>
              </button>
              <button className="btn btn-outline-warning">
                <Link
                  className="text-white text-decoration-none"
                  to="/register"
                >
                  Cadastrar
                </Link>
              </button>
            </div>
          )}
        </div>
      </header>
      <main className="container">
        <div className="d-flex align-items-center justify-content-between mb-4">
          <h2 className="h4 fw-semibold mb-0">Todos os projetos</h2>
          <span className="text-warning small fw-semibold">
            {projects.length} {projects.length === 1 ? "projeto" : "projetos"}
          </span>
        </div>

        {loadingProjects ? (
          <div className="text-center py-5">
            <div className="spinner-border text-warning mb-3" role="status" />
            <p className="text-white-50 mb-0">Carregando projetos...</p>
          </div>
        ) : projects.length === 0 ? (
          <div
            id="no-projects-board"
            className="text-center py-5 rounded-2 border"
          >
            <h4 className="text-warning mb-2">Nenhum projeto encontrado</h4>
            <p className="text-white-50 mb-0">
              Novos projetos aparecerão aqui quando forem cadastrados.
            </p>
          </div>
        ) : (
          <div className="row g-4">
            {projects.map((project) => {
              project.percentage = Math.trunc(project.percentage * 100) / 100;

              return (
                <div className="col-12 col-md-6 col-xl-4" key={project.id}>
                  <Link
                    to={`/projects/${project.id}`}
                    className="text-decoration-none d-block h-100"
                  >
                    <article className="card h-100">
                      <div className="card-body p-4 d-flex flex-column">
                        <div className="d-flex align-items-start justify-content-between gap-3 mb-3 card-fast-info">
                          <div>
                            <h3 className="h5 fw-bold mb-1">{project.nome}</h3>
                            <p className="small text-warning mb-0">
                              por {project.criadorNome}
                            </p>
                          </div>
                          <span className="badge rounded-pill text-dark">
                            {project.percentage}%
                          </span>
                        </div>

                        <p className="text-white-50 flex-grow-1 mb-4">
                          {project.descricao}
                        </p>

                        <div className="mb-3">
                          <div
                            className="progress bg-dark"
                            style={{ height: "8px" }}
                          >
                            <div
                              className="progress-bar"
                              role="progressbar"
                              style={{
                                width: `${project.percentage}%`,
                              }}
                              aria-valuenow={project.percentage}
                              aria-valuemin={0}
                              aria-valuemax={100}
                            />
                          </div>
                        </div>

                        <div className="row g-3 small">
                          <div className="col-6">
                            <span className="d-block text-white-50">Meta</span>
                            <strong>{formatCurrency(project.meta)}</strong>
                          </div>
                          <div className="col-6">
                            <span className="d-block text-white-50">
                              Arrecadado
                            </span>
                            <strong style={{ color: "#ffc107" }}>
                              {formatCurrency(project.arrecadado)}
                            </strong>
                          </div>
                          <div className="col-12 pt-2 border-top">
                            <span className="text-white-50">Início: </span>
                            <strong>
                              {new Date(project.dataCriacao).toLocaleDateString(
                                "pt-BR",
                              )}
                            </strong>
                          </div>
                        </div>
                      </div>
                    </article>
                  </Link>
                </div>
              );
            })}
          </div>
        )}
      </main>
    </S.Div>
  );
}
