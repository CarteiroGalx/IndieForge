import axios from 'axios'
import { useEffect, useState } from 'react'

interface Project {
    id: string
    nome: string
    descricao: string
    meta: number
    arrecadado: number
    dataCriacao: string
    criadorNome: string
}

export default function Home() {
    const [projects, setProjects] = useState<Project[]>([])
    const [userName, setUserName] = useState<string | null>(null)
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

    const getProgress = (project: Project) => {
        if (!project.meta) return 0
        return Math.min(Math.round((project.arrecadado / project.meta) * 100), 100)
    }

    return (
        <main className="min-vh-100 text-white" style={{ backgroundColor: '#070707' }}>
            <section className="border-bottom" style={{ borderColor: '#242424' }}>
                <div className="container py-4">
                    <div className="d-flex flex-column flex-md-row align-items-md-center justify-content-between gap-3">
                        <div>
                            <span className="badge rounded-pill text-dark mb-3" style={{ backgroundColor: '#ffc107' }}>
                                IndieForge
                            </span>
                            <h1 className="display-6 fw-bold mb-2" style={{ color: '#ff8a00' }}>
                                Projetos em destaque
                            </h1>
                            <p className="text-white-50 mb-0">
                                Conheça os projetos independentes que estão buscando apoio.
                            </p>
                        </div>

                        <div className="px-3 py-2 rounded-2 border" style={{ borderColor: '#2f2f2f', backgroundColor: '#111111' }}>
                            {userName ? (
                                <span className="small text-white-50">
                                    Bem-vindo, <strong className="text-warning">{userName}</strong>
                                </span>
                            ) : (
                                <span className="small text-white-50">Você não está logado.</span>
                            )}
                        </div>
                    </div>
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
                            const progress = getProgress(project)

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
                                                    {progress}%
                                                </span>
                                            </div>

                                            <p className="text-white-50 flex-grow-1 mb-4">{project.descricao}</p>

                                            <div className="mb-3">
                                                <div className="progress bg-dark" style={{ height: '8px' }}>
                                                    <div
                                                        className="progress-bar"
                                                        role="progressbar"
                                                        style={{ width: `${progress}%`, backgroundColor: '#ff8a00' }}
                                                        aria-valuenow={progress}
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
