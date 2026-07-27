import React, { useState } from 'react'
import logo from '../assets/logo.png'
import { Link } from 'react-router-dom'
import axios from 'axios'

export default function Login() {
  const [username, setUsername] = useState('')
  const [password, setPassword] = useState('')
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState<string | null>(null)

  const handleLogin = async (event: React.SubmitEvent<HTMLFormElement>) => {
    event.preventDefault()
    setLoading(true)
    setError(null)
    console.log('login submit:', { username, password })
    try {
      const response = await axios.post('http://localhost:5259/api/auth/login', { username, password })
      localStorage.setItem('token', response.data.message)
      console.log(localStorage.getItem('token'))
    } catch (err: any) {
      setError(err.response?.data?.message || 'Erro ao fazer login. Tente novamente.')
    }
    setLoading(false)
  }

  return (
    <div className="d-flex align-items-center justify-content-center vh-100" style={{ backgroundColor: '#0a0a0a' }}>
      <div className="card shadow-lg my-3" style={{ width: '380px', border: '0', borderRadius: '1rem', backgroundColor: '#111111' }}>
        {loading && (
          <div className="rounded h-100 w-100 text-white d-flex" style={{ backgroundColor: '#111111', position: 'absolute', top: 0, left: 0, zIndex: 10, justifyContent: 'center', alignItems: 'center' }}>
            <h2>Carregando...</h2>
          </div>
        )}
        <div className="card-body p-3">
          <div className="col-12 text-center mb-4">
            <img className='col-4' src={logo} alt="IndieForge Logo" />
            <h2 className="fw-bold" style={{ color: '#ffaa00' }}>
              IndieForge
            </h2>
            <p className="text-white-50">Faça login para acessar sua área de creator.</p>
          </div>
          <form onSubmit={handleLogin}>
            <div className="mb-3">
              <label htmlFor="username" className="form-label text-white">
                Nome de Usuário
              </label>
              <input
                type="text"
                id="username"
                className="form-control bg-dark text-white border-secondary"
                value={username}
                onChange={(event) => setUsername(event.target.value)}
                placeholder="Digite seu nome de usuário"
                required
              />
            </div>

            <div className="mb-4">
              <label htmlFor="password" className="form-label text-white">
                Senha
              </label>
              <input
                type="password"
                id="password"
                className="form-control bg-dark text-white border-secondary"
                value={password}
                onChange={(event) => setPassword(event.target.value)}
                placeholder="Digite sua senha"
                required
              />
            </div>

            <div className="fs-6 mb-3 text-center">
              {error && <p className="text-danger">{error}</p>}
            </div>
            <div className="d-grid gap-2 mb-3">
              <button type="submit" className="btn btn-warning btn-lg text-uppercase fw-semibold">
                Entrar
              </button>
            </div>

            <div className="text-center text-white-50 mb-0">
              <small>Ainda não tem conta? <Link to="/register">Clique aqui</Link></small>
            </div>
          </form>
        </div>
      </div>
    </div>
  )
}
