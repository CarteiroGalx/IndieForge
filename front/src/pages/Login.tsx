import React, { useState } from 'react'
import logo from '../assets/logo.png'

export default function Login() {
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')

  const handleLogin = (event: React.SubmitEvent) => {
    event.preventDefault()
    // TODO: implementar lógica de autenticação
    console.log('login submit:', { email, password })
  }

  const handleRegister = () => {
    // TODO: navegar para tela de registro ou abrir modal de inscrição
    console.log('register action triggered')
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
              <small>Ainda não tem conta?</small>
            </div>
            <div className="d-grid gap-2 mt-2">
              <button type="button" className="btn btn-outline-warning text-white" onClick={handleRegister}>
                Registrar-se
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  )
}
