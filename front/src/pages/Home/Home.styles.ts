import styled from 'styled-components'

export const Div = styled.div`
    background-color: #070707;

    header{
        background-color: pink;
    }

    #icon-profile{
        width: 50px;
        height: 50px;
        font-size: 1.25rem;
        color: #ffc107;
        background-color: #5f6163;
        border: 2px solid #ffc107;
    }

    #icon-profile:hover{
        cursor: pointer;
        background-color: #ffc107;
        color: black;
    }

    main{
        padding-top: 90px;
    }

    #no-projects-board{
        border-color: #2f2f2f; 
        background-color: #111111;
    }

    article{
        background-color: #121212;
        color: white;
    }

    .card-body .card-fast-info span{
        background-color: #ffc107
    }

    .progress-bar{
        background-color: #ff9c07
    }
    
`
