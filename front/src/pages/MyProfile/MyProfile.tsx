import axios from 'axios'
import { useEffect, useState } from 'react'

interface Project {
  id?: string
  nome?: string
  descricao?: string
  meta?: number
  arrecadado?: number
  dataInicio?: string
}

interface UserProfile {
  nome: string
  email: string
  emailConfirmado: boolean
  projetos: Project[]
  totalArrecadadoEmContribuicoes: number
}

export default function MyProfile() {
  const [profile, setProfile] = useState<UserProfile | null>(null)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  useEffect(() => {
    axios.get('http://localhost:5259/api/me', {
      headers: {
        'Authorization': `Bearer ${localStorage.getItem('token')}`
      }
    }).then(response => {
      setProfile(response.data)
    }).catch(err => {
      console.error('Error fetching profile:', err)
      setError('Não foi possível carregar as informações do perfil.')
    }).finally(() => {
      setLoading(false)
    })
  }, [])

  const formatCurrency = (value: number) => {
    return new Intl.NumberFormat('pt-BR', {
      style: 'currency',
      currency: 'BRL'
    }).format(value)
  }

  const getInitials = (name: string) => {
    return name
      .split(' ')
      .filter(Boolean)
      .slice(0, 2)
      .map(part => part[0])
      .join('')
      .toUpperCase()
  }

  return (
    <main className="min-vh-100 text-white" style={{ backgroundColor: '#070707' }}>
      <section className="border-bottom" style={{ borderColor: '#242424' }}>
        <div className="container py-4">
          <div className="d-flex align-items-center justify-content-between gap-3">
            <div>
              <span className="badge rounded-pill text-dark mb-3" style={{ backgroundColor: '#ffc107' }}>
                Meu Perfil
              </span>
              <h1 className="display-6 fw-bold mb-1" style={{ color: '#ff8a00' }}>
                Perfil do usuário
              </h1>
              <p className="text-white-50 mb-0">Informações da sua conta IndieForge.</p>
            </div>
          </div>
        </div>
      </section>

      <section className="container py-5">
        {loading ? (
          <div className="text-center py-5">
            <div className="spinner-border text-warning mb-3" role="status" />
            <p className="text-white-50 mb-0">Carregando perfil...</p>
          </div>
        ) : error ? (
          <div className="text-center py-5 rounded-2 border" style={{ borderColor: '#2f2f2f', backgroundColor: '#111111' }}>
            <h2 className="h5 text-warning mb-2">Erro ao carregar</h2>
            <p className="text-white-50 mb-0">{error}</p>
          </div>
        ) : profile && (
          <div className="row g-4 align-items-start">
            <aside className="col-12 col-lg-4">
              <div className="position-sticky" style={{ top: '24px' }}>
                <div
                  className="rounded-circle d-flex align-items-center justify-content-center fw-bold mb-4"
                  style={{
                    width: '180px',
                    height: '180px',
                    backgroundColor: '#ff8a00',
                    color: '#070707',
                    fontSize: '3.5rem',
                    border: '4px solid #ffc107'
                  }}
                >
                  {getInitials(profile.nome)}
                </div>

                <h2 className="h3 fw-bold mb-1">{profile.nome}</h2>
                <p className="text-white-50 mb-3">{profile.email}</p>

                <div className="d-flex flex-wrap gap-2 mb-4">
                  <span
                    className={`badge rounded-pill ${profile.emailConfirmado ? 'text-dark' : 'text-white'}`}
                    style={{ backgroundColor: profile.emailConfirmado ? '#ffc107' : '#2f2f2f' }}
                  >
                    {profile.emailConfirmado ? 'E-mail confirmado' : 'E-mail não confirmado'}
                  </span>
                  <span className="badge rounded-pill text-dark" style={{ backgroundColor: '#ff8a00' }}>
                    {profile.projetos.length} {profile.projetos.length === 1 ? 'projeto' : 'projetos'}
                  </span>
                </div>

                <div className="rounded-2 border p-3" style={{ borderColor: '#2f2f2f', backgroundColor: '#111111' }}>
                  <span className="d-block small text-white-50 mb-1">Total arrecadado em contribuições</span>
                  <strong className="h4 mb-0" style={{ color: '#ffc107' }}>
                    {formatCurrency(profile.totalArrecadadoEmContribuicoes)}
                  </strong>
                </div>
              </div>
            </aside>

            <div className="col-12 col-lg-8">
              <div className="rounded-2 border mb-4" style={{ borderColor: '#2f2f2f', backgroundColor: '#111111' }}>
                <div className="p-4 border-bottom" style={{ borderColor: '#2f2f2f' }}>
                  <h3 className="h5 fw-semibold mb-0">Visão geral</h3>
                </div>
                <div className="row g-0">
                  <div className="col-12 col-md-4 p-4 border-end" style={{ borderColor: '#2f2f2f' }}>
                    <span className="d-block text-white-50 small mb-1">Nome</span>
                    <strong>{profile.nome}</strong>
                  </div>
                  <div className="col-12 col-md-4 p-4 border-end" style={{ borderColor: '#2f2f2f' }}>
                    <span className="d-block text-white-50 small mb-1">E-mail</span>
                    <strong className="text-break">{profile.email}</strong>
                  </div>
                  <div className="col-12 col-md-4 p-4">
                    <span className="d-block text-white-50 small mb-1">Status</span>
                    <strong style={{ color: profile.emailConfirmado ? '#ffc107' : '#ff8a00' }}>
                      {profile.emailConfirmado ? 'Confirmado' : 'Pendente'}
                    </strong>
                  </div>
                </div>
              </div>

              <div>
                <div className="d-flex align-items-center justify-content-between mb-3">
                  <h3 className="h5 fw-semibold mb-0">Projetos</h3>
                  <span className="text-warning small fw-semibold">
                    {profile.projetos.length} {profile.projetos.length === 1 ? 'item' : 'itens'}
                  </span>
                </div>

                {profile.projetos.length === 0 ? (
                  <div className="rounded-2 border p-4 text-center" style={{ borderColor: '#2f2f2f', backgroundColor: '#111111' }}>
                    <h4 className="h6 text-warning mb-2">Nenhum projeto cadastrado</h4>
                    <p className="text-white-50 mb-0">Os seus projetos aparecerão nesta área do perfil.</p>
                  </div>
                ) : (
                  <div className="d-grid gap-3">
                    {profile.projetos.map((project, index) => (
                      <article
                        className="rounded-2 border p-4"
                        style={{ borderColor: '#2f2f2f', backgroundColor: '#111111' }}
                        key={project.id || `${project.nome}-${index}`}
                      >
                        <div className="d-flex flex-column flex-md-row justify-content-between gap-3">
                          <div>
                            <h4 className="h5 fw-semibold mb-2">{project.nome || 'Projeto sem nome'}</h4>
                            {project.descricao && <p className="text-white-50 mb-0">{project.descricao}</p>}
                          </div>
                          <div className="text-md-end small">
                            {typeof project.meta === 'number' && (
                              <div>
                                <span className="text-white-50">Meta: </span>
                                <strong>{formatCurrency(project.meta)}</strong>
                              </div>
                            )}
                            {typeof project.arrecadado === 'number' && (
                              <div>
                                <span className="text-white-50">Arrecadado: </span>
                                <strong style={{ color: '#ffc107' }}>{formatCurrency(project.arrecadado)}</strong>
                              </div>
                            )}
                            {project.dataInicio && (
                              <div>
                                <span className="text-white-50">Início: </span>
                                <strong>{new Date(project.dataInicio).toLocaleDateString('pt-BR')}</strong>
                              </div>
                            )}
                          </div>
                        </div>
                      </article>
                    ))}
                  </div>
                )}
              </div>
            </div>
          </div>
        )}
      </section>
    </main>
  )
}
