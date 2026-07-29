import axios from 'axios'
import React, { useEffect, useState } from 'react'

export default function Home() {
    const [projects, setProjects] = useState<Project[]>([])
    const [userName, setUserName] = useState<string | null>(null)

    interface Project {
        id: string
        nome: string
        descricao: string
        meta: number
        arrecadado: number
        dataInicio: string
        criadorNome: string
    }

    useEffect(() => {
        axios.get('http://localhost:5259/api/projects')
            .then(response => {
                console.log('Projects fetched:', response.data);
                setProjects(response.data);
            })
            .catch(error => {
                console.error('Error fetching projects:', error);
            });
    }, []);

    useEffect(() => {
        axios.get('http://localhost:5259/api/check-auth', {
            headers: {
                'Authorization': `Bearer ${localStorage.getItem('token')}`
            }
        }).then(response => {
            console.log('Auth check response:', response.data);
            console.log(localStorage.getItem('token'));
            setUserName(response.data.userName);
        }).catch(error => {
            console.error('Error checking auth:', error);
            setUserName(null);
            localStorage.removeItem('token');
        })
    }, []);

    return (
        <div>
            {projects.map(project => (
                <div key={project.id}>
                    <h2>{project.nome}</h2>
                    <p>{project.descricao}</p>
                    <p>Meta: {project.meta}</p>
                    <p>Arrecadado: {project.arrecadado}</p>
                    <p>Data de Início: {project.dataInicio}</p>
                    <p>Criador: {project.criadorNome}</p>
                </div>
            ))}
            </div>
            <div className="bg-dark text-white p-3">
                {userName ? (
                    <p>Bem-vindo, {userName}!</p>
                ) : (
                    <p>Você não está logado.</p>
                )}
            </div>
        </div>
    )
}
