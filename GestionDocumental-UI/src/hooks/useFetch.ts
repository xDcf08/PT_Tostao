import { useEffect, useState } from "react";
import axios from 'axios'
import type { Document } from "../models/document";

interface FetchResult {
  data: Document[];
  loading: boolean;
  error: string | null;
}

export const useFetch = (page: number, autorFilter: string) : FetchResult => {
  const [data, setData] = useState<Document[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {

    const contoller = new AbortController();

    const fetchDocuments = async () => {
      setLoading(true);
      setError(null);
      
      try {
        const apiUrl = 'http://localhost:5044/api/v1/documents';
        const endpoint = autorFilter ? `${apiUrl}/buscar` : apiUrl;
        
        const params = {
          pageNumber: page,
          pageSize: 10,
          autor: autorFilter || undefined
        };

        const response = await axios.get<Document[]>(endpoint, { params, signal: contoller.signal });
        setData(response.data);
      } catch (err) {
        if(axios.isCancel(err)){
          console.log("Request canceled:", err.message)
        }else{
          setError('Error al cargar los documentos. Revisa la consola para más detalles.');
          console.error(err);
        }
      } finally {
        setLoading(false);
      }
    };

    const timerId = setTimeout(() => {
      fetchDocuments();
    }, 500)

    return () => {
      clearTimeout(timerId);
      contoller.abort();
    }
  },[page, autorFilter])

  return {
    data,
    loading,
    error
  }
}