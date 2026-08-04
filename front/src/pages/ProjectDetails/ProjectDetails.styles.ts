import styled from "styled-components";

export const Page = styled.div`
  min-height: 100vh;
  background: #070707;
  color: #ffffff;

  .modal{
    background: rgba(0, 0, 0, 0.6);
  }

  main {
    padding-bottom: 56px;
    padding-top: 32px;
  }

  .back-link {
    color: #ffc107;
    display: inline-block;
    font-weight: 700;
    margin-bottom: 24px;
    text-decoration: none;
  }

  .back-link:hover {
    color: #ff9c07;
  }

  .project-hero {
    align-items: stretch;
    display: grid;
    gap: 24px;
    grid-template-columns: minmax(0, 1fr) 320px;
    margin-bottom: 24px;
  }

  .project-hero h1 {
    font-size: clamp(2rem, 5vw, 4.5rem);
    font-weight: 800;
    line-height: 1;
    margin: 16px 0;
  }

  .project-hero p {
    color: #d6d6d6;
    font-size: 1.1rem;
    line-height: 1.7;
    margin: 0;
    max-width: 760px;
  }

  .status-pill {
    background: #ff9c07;
    border-radius: 999px;
    color: #070707;
    display: inline-flex;
    font-size: 0.8rem;
    font-weight: 800;
    padding: 8px 14px;
    text-transform: uppercase;
  }

  .summary-card,
  .stats-grid article,
  .contribution-card,
  .empty-state {
    background: #121212;
    border: 1px solid #2f2f2f;
    border-radius: 8px;
  }

  .summary-card {
    display: flex;
    flex-direction: column;
    justify-content: center;
    padding: 28px;
  }

  .summary-card span,
  .stats-grid span,
  .contribution-card span {
    color: #a8a8a8;
  }

  .summary-card strong {
    color: #ffc107;
    font-size: 2rem;
    line-height: 1.1;
    margin: 8px 0;
  }

  .summary-card small {
    color: #ffffff;
  }

  .progress-section {
    margin-bottom: 24px;
  }

  .progress-heading {
    align-items: center;
    display: flex;
    justify-content: space-between;
    margin-bottom: 10px;
  }

  .progress-heading span {
    color: #d6d6d6;
    font-weight: 700;
  }

  .progress-heading strong {
    color: #ffc107;
  }

  .progress-track {
    background: #1d1d1d;
    border-radius: 999px;
    height: 12px;
    overflow: hidden;
  }

  .progress-fill {
    background: linear-gradient(90deg, #ff8a00, #ffc107);
    height: 100%;
  }

  .stats-grid {
    display: grid;
    gap: 16px;
    grid-template-columns: repeat(4, minmax(0, 1fr));
    margin-bottom: 32px;
  }

  .stats-grid article {
    display: flex;
    flex-direction: column;
    gap: 8px;
    padding: 20px;
  }

  .stats-grid strong {
    color: #ffffff;
    font-size: 1.2rem;
  }

  .section-heading {
    align-items: center;
    display: flex;
    justify-content: space-between;
    margin-bottom: 16px;
  }

  .section-heading h2 {
    font-size: 1.4rem;
    margin: 0;
  }

  .section-heading span {
    color: #ffc107;
    font-weight: 800;
  }

  .contributions-list {
    display: grid;
    gap: 12px;
  }

  .contribution-card {
    align-items: center;
    display: flex;
    justify-content: space-between;
    padding: 18px 20px;
  }

  .contribution-card div {
    display: flex;
    flex-direction: column;
    gap: 4px;
  }

  .contribution-card > strong {
    color: #ffc107;
  }

  .empty-state {
    padding: 32px;
    text-align: center;
  }

  .empty-state h1 {
    color: #ffc107;
    margin-bottom: 12px;
  }

  .empty-state p {
    color: #cfcfcf;
    margin: 0;
  }

  @media (max-width: 992px) {
    .project-hero,
    .stats-grid {
      grid-template-columns: 1fr 1fr;
    }
  }

  @media (max-width: 768px) {
    .project-hero,
    .stats-grid {
      grid-template-columns: 1fr;
    }

    .summary-card {
      padding: 22px;
    }

    .contribution-card {
      align-items: flex-start;
      flex-direction: column;
      gap: 12px;
    }
  }
`;
