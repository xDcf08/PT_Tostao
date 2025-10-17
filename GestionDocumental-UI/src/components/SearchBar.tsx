interface SearchBarProps {
  filter: string;
  setFilter: (filtro: string) => void;
}

export const SearchBar = ({filter, setFilter} : SearchBarProps) => {
  return (
    <div className="search-bar">
      <input
        type="text"
        value={filter}
        onChange={(e) => setFilter(e.target.value)}
        placeholder="Buscar por autor en tiempo real..."
      />
    </div>
  )
}
