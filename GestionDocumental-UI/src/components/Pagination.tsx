import React from 'react'
interface PaginationProps {
  pagina: number;
  setPagina: React.Dispatch<React.SetStateAction<number>>;
  hayMasPaginas: boolean;
}

export const Pagination = ({ pagina, setPagina, hayMasPaginas } : PaginationProps) => {

  const irPaginaAnterior = () => {
    setPagina((p) => Math.max(p -1, 1))
  }

  const irPaginaSiguiente = () => {
    setPagina((p) => p+1);
  }

  return (
    <div className="pagination">
      <button onClick={irPaginaAnterior} disabled={pagina === 1}>
        Anterior
      </button>
      <span>Página {pagina}</span>
      <button onClick={irPaginaSiguiente} disabled={!hayMasPaginas}>
        Siguiente
      </button>
    </div>
  )
}
