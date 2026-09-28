import './Pagination.css'

type PaginationProps = {
  page: number
  totalPages: number
  onPageChange: (page: number) => void
}

function Pagination({
  page,
  totalPages,
  onPageChange
}: PaginationProps) {

  function mergiLaPagina(pagina: number) {
    if (pagina < 1 || pagina > totalPages) {
      return
    }

    onPageChange(pagina)
  }

  return (
    <nav className="pagination" aria-label="Paginare">
      <button
        type="button"
        onClick={() => mergiLaPagina(page - 1)}
        disabled={page === 1}
      >
        Anterior
      </button>

      {Array.from(
        { length: totalPages },
        (_, index) => index + 1
      ).map(pagina => (
        <button
          key={pagina}
          type="button"
          onClick={() => mergiLaPagina(pagina)}
          disabled={pagina === page}
          className={pagina === page ? 'pagination-page-active' : ''}
        >
          {pagina}
        </button>
      ))}

      <button
        type="button"
        onClick={() => mergiLaPagina(page + 1)}
        disabled={page === totalPages}
      >
        Urmatoarea
      </button>
    </nav>
  )
}

export default Pagination