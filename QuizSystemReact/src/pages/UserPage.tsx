import { useEffect, useContext } from 'react'; // Hooky pro práci s efekty a kontextem
import { QuizContext } from '../context/QuizContext'; // Kontext pro práci s kvízy
import { useNavigate } from 'react-router-dom'; // Hook pro navigaci na různé stránky
import styles from './UserPage.module.css'; // Stylování komponenty

const UserPage = () => {
    const { quizzes, fetchUserQuizzes } = useContext(QuizContext); // Získání kvízů a funkce pro jejich načtení z kontextu
    const navigate = useNavigate(); // Navigace mezi stránkami

    // Načítání kvízů při mountu komponenty
    useEffect(() => {
        fetchUserQuizzes(); // Načtení kvízů uživatele
    }, []); // Efekt běží pouze při prvním renderování

    // Funkce pro navigaci na stránku pro vytvoření nového kvízu
    const handleCreateQuiz = () => {
        navigate('/create'); 
    };

    // Nastavení názvu stránky
    useEffect(() => {
        document.title = "User"; 
    }, []); 

    return (
        <div className={styles.userPage}>
            <h1 className={styles.title}>Your Quizzes</h1>
            <button className={styles.createButton} onClick={handleCreateQuiz}>
                Create New Quiz
            </button>
            <div className={styles.quizzesContainer}>
                {quizzes.length > 0 ? ( // Pokud uživatel má kvízy
                    quizzes.map((quiz) => ( // Pro každý kvíz zobrazíme kartu
                        <div key={quiz.id} className={styles.quizCard}>
                            <h3>{quiz.name}</h3> {/* Název kvízu */}
                            <p>{quiz.description}</p> {/* Popis kvízu */}
                            <div className={styles.buttonsContainer}>
                                <button onClick={() => navigate(`/view/${quiz.id}`)}>View</button> {/* Zobrazit kvíz */}
                                <button onClick={() => navigate(`/edit/${quiz.id}`)}>Edit</button> {/* Upravit kvíz */}
                            </div>
                        </div>
                    ))
                ) : (
                    <p className={styles.emptyMessage}>You have no quizzes yet.</p> // Zpráva, pokud uživatel nemá žádné kvízy
                )}
            </div>
        </div>
    );
};

export default UserPage;
