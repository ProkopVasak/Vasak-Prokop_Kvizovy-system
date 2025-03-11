import { useContext, useState, useEffect } from 'react';
import { QuizContext } from '../context/QuizContext'; // Importujeme kontext pro kvízy
import { useNavigate } from 'react-router-dom'; // Hook pro navigaci na jinou stránku
import styles from './HomePage.module.css';

const ITEMS_PER_PAGE = 18; // Nastavíme konstantu pro počet kvízů na stránce

const HomePage = () => {
    const { quizzes } = useContext(QuizContext); // Získáme kvízy z kontextu
    const [filteredQuizzes, setFilteredQuizzes] = useState(quizzes); // Stav pro uložení filtrovaných kvízů
    const [page, setPage] = useState(1); // Stav pro aktuální stránku
    const [searchQuery, setSearchQuery] = useState(''); // Stav pro uchování hledaného textu
    const navigate = useNavigate(); // Hook pro navigaci

    useEffect(() => {
        // Tento useEffect se spustí při změně hledaného textu nebo dat v kontextu
        if (searchQuery) {
            // Filtrování kvízů podle názvu nebo popisu
            const filtered = quizzes.filter((quiz) =>
                quiz.name.toLowerCase().includes(searchQuery.toLowerCase()) ||
                quiz.description.toLowerCase().includes(searchQuery.toLowerCase())
            );
            setFilteredQuizzes(filtered); // Nastavení filtrovaných kvízů
            setPage(1); // Resetování stránky na 1 při změně hledaného textu
        } else {
            setFilteredQuizzes(quizzes); // Pokud není zadán žádný text, zobrazíme všechny kvízy
        }
    }, [searchQuery, quizzes]); // Tento useEffect závisí na vyhledávacím dotazu nebo na kvízech v kontextu

    const startIndex = (page - 1) * ITEMS_PER_PAGE; // Počáteční index pro stránkování
    const selectedQuizzes = filteredQuizzes.slice(startIndex, startIndex + ITEMS_PER_PAGE); // Výběr kvízů pro aktuální stránku

    // Funkce pro přechod na další stránku
    const handleNextPage = () => {
        if (startIndex + ITEMS_PER_PAGE < filteredQuizzes.length) {
            setPage((prevPage) => prevPage + 1); // Zvyšujeme stránku o 1
        }
    };

    // Funkce pro přechod na předchozí stránku
    const handlePreviousPage = () => {
        setPage((prevPage) => Math.max(prevPage - 1, 1)); // Snižujeme stránku o 1, ale nikdy ne na hodnotu menší než 1
    };

    // Funkce pro navigaci na stránku kvízu při kliknutí na kvíz
    const handleQuizClick = (id: string) => {
        navigate(`/quiz/${id}`); // Přesměrování na stránku kvízu podle jeho id
    };

    // Funkce pro změnu vyhledávacího textu
    const handleSearchChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        setSearchQuery(event.target.value); // Aktualizace vyhledávacího dotazu
    };

    // Změna názvu stránky při renderování komponenty
    useEffect(() => {
        document.title = "Home"; 
    }, []);

    return (
        <div className={styles.homePage}>
            <h1 className={styles.title}>Quizzes</h1>
            {/* Vyhledávací pole pro zadání hledaného textu */}
            <input
                className={styles.searchInput}
                type="text"
                placeholder="Search quizzes"
                value={searchQuery}
                onChange={handleSearchChange} // Aktualizace vyhledávacího dotazu při změně
            />
            {/* Zobrazení kvízů v mřížce */}
            <div className={styles.quizGrid}>
                {selectedQuizzes.map((quiz) => (
                    <div key={quiz.id} className={styles.quizCard}>
                        <h3>{quiz.name}</h3>
                        <p>{quiz.description}</p>
                        <button onClick={() => handleQuizClick(quiz.id)}>Play</button>
                    </div>
                ))}
            </div>
            {/* Stránkování */}
            <div className={styles.pagination}>
                <button onClick={handlePreviousPage} disabled={page === 1}>
                    Previous
                </button>
                <span>Page {page}</span>
                <button
                    onClick={handleNextPage}
                    disabled={startIndex + ITEMS_PER_PAGE >= filteredQuizzes.length}
                >
                    Next
                </button>
            </div>
        </div>
    );
};

export default HomePage;
