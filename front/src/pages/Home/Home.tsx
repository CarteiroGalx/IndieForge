import axios from 'axios'
import { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'

interface Project {
    id: string
    nome: string
    descricao: string
    meta: number
    arrecadado: number
    percentage: number
    dataCriacao: string
    criadorNome: string
}

export default function Home() {
    const [projects, setProjects] = useState<Project[]>([])
    const [userName, setUserName] = useState<string | null>(null)
    const [projectNameSearch, setProjecNameSearch] = useState('')
    const [loadingProjects, setLoadingProjects] = useState(true)

    useEffect(() => {
        axios.get('http://localhost:5259/api/projects')
            .then(response => {
                console.log('Projects fetched:', response.data)
                setProjects(response.data)
            })
            .catch(error => {
                console.error('Error fetching projects:', error)
            })
            .finally(() => {
                setLoadingProjects(false)
            })
    }, [])

    
  const getInitials = (name: string) => {
    return name
      .split(' ')
      .filter(Boolean)
      .slice(0, 2)
      .map(part => part[0])
      .join('')
      .toUpperCase()
  }

    useEffect(() => {
        axios.get('http://localhost:5259/api/check-auth', {
            headers: {
                'Authorization': `Bearer ${localStorage.getItem('token')}`
            }
        }).then(response => {
            console.log('Auth check response:', response.data)
            console.log(localStorage.getItem('token'))
            setUserName(response.data.userName)
        }).catch(error => {
            console.error('Error checking auth:', error)
            setUserName(null)
            localStorage.removeItem('token')
        })
    }, [])

    const formatCurrency = (value: number) => {
        return new Intl.NumberFormat('pt-BR', {
            style: 'currency',
            currency: 'BRL'
        }).format(value)
    }

    const getProjectsByName = async (event: React.SubmitEvent<HTMLFormElement>) => {
        event.preventDefault()
        setLoadingProjects(true)
        axios.get('http://localhost:5259/api/projects?' + 'name=' + projectNameSearch)
        .then(response => {
            setProjects(response.data)
            console.log(response.data)
        })
        .catch(err => {
            console.log("Deu errado! Erro: " + err)
        })
        .finally(() => {
            setLoadingProjects(false)
        })
    }

    return (
        <main className="min-vh-100 text-white" style={{ backgroundColor: '#070707' }}>
            <section className="border-bottom" style={{ borderColor: '#242424' }}>
                <div className="container py-2">
                    <header className="col-12 py-1">
                        <div className="d-flex align-items-center justify-content-between">
                            <div className="row g-2 col-9">
                                <form onSubmit={getProjectsByName}>
                                    <input type='text' placeholder='Pesquisar' value={projectNameSearch} onChange={(e) => setProjecNameSearch(e.target.value)}id="search-input" className="form-control bg-dark border-0 text-white"></input>
                                </form>
                            </div>
                                {userName ? (
                                    <Link to="/profile" className="text-decoration-none border border-0 col-auto">
                                                <div
                                                className="rounded-circle d-flex align-items-center justify-content-center fw-bold"
                                                style={{
                                                    width: '50px',
                                                    height: '50px',
                                                    backgroundColor: '#ff8a00',
                                                    color: '#070707',
                                                    fontSize: '1.25rem',
                                                    border: '2px solid #ffc107',
                                                }}
                                                >
                                                {getInitials(userName)}
                                            </div>
                                    </Link>
                                ) : (
                                    <div className="d-flex gap-2 col-auto">
                                        <button className="btn btn-warning"><Link className="text-white text-decoration-none" to="/">Entrar</Link></button>
                                        <button className="btn btn-outline-warning"><Link className="text-white text-decoration-none" to="/register">Cadastrar</Link></button>
                                    </div>
                                )}
                            </div>
                    </header>
                </div>
            </section>
            <section className="container py-5">
                <div className="d-flex align-items-center justify-content-between mb-4">
                    <h2 className="h4 fw-semibold mb-0">Todos os projetos</h2>
                    <span className="text-warning small fw-semibold">
                        {projects.length} {projects.length === 1 ? 'projeto' : 'projetos'}
                    </span>
                </div>

                {loadingProjects ? (
                    <div className="text-center py-5">
                        <div className="spinner-border text-warning mb-3" role="status" />
                        <p className="text-white-50 mb-0">Carregando projetos...</p>
                    </div>
                ) : projects.length === 0 ? (
                    <div className="text-center py-5 rounded-2 border" style={{ borderColor: '#2f2f2f', backgroundColor: '#111111' }}>
                        <h3 className="h5 text-warning mb-2">Nenhum projeto encontrado</h3>
                        <p className="text-white-50 mb-0">Novos projetos aparecerão aqui quando forem cadastrados.</p>
                    </div>
                ) : (
                    <div className="row g-4">
                        {projects.map(project => {
                            project.percentage = Math.trunc(project.percentage * 100) / 100;

                            return (
                                <div className="col-12 col-md-6 col-xl-4" key={project.id}>
                                    <article
                                        className="card h-100 shadow-sm border-0"
                                        style={{ backgroundColor: '#121212', color: '#ffffff' }}
                                    >
                                        <div className="card-body p-4 d-flex flex-column">
                                            <div className="d-flex align-items-start justify-content-between gap-3 mb-3">
                                                <div>
                                                    <h3 className="h5 fw-bold mb-1">{project.nome}</h3>
                                                    <p className="small text-warning mb-0">por {project.criadorNome}</p>
                                                </div>
                                                <span className="badge rounded-pill text-dark" style={{ backgroundColor: '#ffb000' }}>
                                                    {project.percentage}%
                                                </span>
                                            </div>

                                            <p className="text-white-50 flex-grow-1 mb-4">{project.descricao}</p>

                                            <div className="mb-3">
                                                <div className="progress bg-dark" style={{ height: '8px' }}>
                                                    <div
                                                        className="progress-bar"
                                                        role="progressbar"
                                                        style={{ width: `${project.percentage}%`, backgroundColor: '#ff8a00' }}
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
                                                    <span className="d-block text-white-50">Arrecadado</span>
                                                    <strong style={{ color: '#ffc107' }}>{formatCurrency(project.arrecadado)}</strong>
                                                </div>
                                                <div className="col-12 pt-2 border-top" style={{ borderColor: '#2f2f2f' }}>
                                                    <span className="text-white-50">Início: </span>
                                                    <strong>{new Date(project.dataCriacao).toLocaleDateString('pt-BR')}</strong>
                                                </div>
                                            </div>
                                        </div>
                                    </article>
                                </div>
                            )
                        })}
                    </div>
                )}
            </section>
        </main>
    )
}
