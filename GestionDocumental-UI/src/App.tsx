import { useState } from "react";
import "./App.css";
import { DocumentList } from "./components/DocumentList";
import { Pagination } from "./components/Pagination";
import { SearchBar } from "./components/SearchBar";
import { useFetch } from "./hooks/useFetch";

function App() {
  const [page, setPage] = useState<number>(1);
  const [autorFilter, setAutorFilter] = useState<string>("");
  const { data: documentos, loading, error } = useFetch(page, autorFilter);

  const hayMasPaginas = documentos.length === 10;

  return (
    <div className="container">
      <header>
        <h1>Sistema de Gestión Documental</h1>
        <SearchBar filter={autorFilter} setFilter={setAutorFilter} />
      </header>

      <main>
        {loading && <p className="loading-message">Cargando documentos...</p>}
        {error && <p className="error-message">{error}</p>}
        {!loading && !error && <DocumentList documents={documentos} />}
      </main>

      <footer>
        <Pagination
          pagina={page}
          setPagina={setPage}
          hayMasPaginas={hayMasPaginas}
        />
      </footer>
    </div>
  );
}

export default App;
