import type { Document } from '../models/document'

interface DocumentListProps {
  documents: Document[]
}

export const DocumentList = ({documents} : DocumentListProps) => {
  return (
    <table>
      <thead>
        <tr>
          <th>Título</th>
          <th>Autor</th>
          <th>Tipo</th>
          <th>Estado</th>
          <th>Fecha de Registro</th>
        </tr>
      </thead>
      <tbody>
        {documents.map((doc) => (
          <tr key={doc.id}>
            <td>{doc.titulo}</td>
            <td>{doc.autor}</td>
            <td>{doc.tipo}</td>
            <td>{doc.estado}</td>
            <td>{new Date(doc.fechaRegistro).toLocaleDateString()}</td>
          </tr>
        ))}
      </tbody>
    </table>
  )
}
