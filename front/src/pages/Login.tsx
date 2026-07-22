import React, { useState } from 'react'
import logo from '../assets/logo.png'
import { Link } from 'react-router-dom'

export default function Login() {
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')

  const handleLogin = (event: React.SubmitEvent<HTMLFormElement>) => {
    event.preventDefault()
    // TODO: implementar lógica de autenticação
    console.log('login submit:', { email, password })
  }

  return (
    <div className="d-flex align-items-center justify-content-center vh-100" style={{ backgroundColor: '#0a0a0a' }}>
      <div className="card shadow-lg my-3" style={{ width: '380px', border: '0', borderRadius: '1rem', backgroundColor: '#111111' }}>
        <div className="card-body p-3">
          <div className="col-12 text-center mb-4">
            <img className='col-4' src={logo} alt="IndieForge Logo" />
            <h2 className="fw-bold" style={{ color: '#ff8c00' }}>
              IndieForge
            </h2>
            <p className="text-white-50">Faça login para acessar sua área de creator.</p>
          </div>

          <form onSubmit={handleLogin}>
            <div className="mb-3">
              <label htmlFor="email" className="form-label text-white">
                E-mail
              </label>
              <input
                type="email"
                id="email"
                className="form-control bg-dark text-white border-secondary"
                value={email}
                onChange={(event) => setEmail(event.target.value)}
                placeholder="Digite seu e-mail"
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
