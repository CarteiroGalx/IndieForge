import axios from 'axios'
import React, { useEffect, useState } from 'react'

export default function Home() {
    const [projects, setProjects] = useState<Project[]>([])

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
    )
}
