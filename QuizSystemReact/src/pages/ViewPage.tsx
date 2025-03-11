import { useEffect, useState } from "react"; // Hooky pro práci s efekty a stavem
import { useParams, useNavigate } from "react-router-dom"; // Hooky pro práci s parametry URL a navigaci
import styles from "./ViewPage.module.css"; // Stylování komponenty

export interface Answer {
    id: string; // ID odpovědi
    text: string; // Text odpovědi
    isCorrect: boolean; // Určuje, zda je odpověď správná
}

export interface Question {
    id: string; // ID otázky
    text: string; // Text otázky
    answers: Answer[]; // Seznam odpovědí k otázce
}

export interface Quiz {
    id: string; // ID kvízu
    name: string; // Název kvízu
    description: string; // Popis kvízu
    questions: Question[]; // Seznam otázek v kvízu
}

const ViewQuizPage = () => {
    const { id } = useParams(); // Získání ID kvízu z URL
    const navigate = useNavigate(); // Navigace mezi stránkami
    const [quiz, setQuiz] = useState<Quiz | null>(null); // Stav pro kvíz
    const [loading, setLoading] = useState(true); // Stav pro načítání

    // Efekt pro načtení kvízu z API
    useEffect(() => {
        const fetchQuiz = async () => {
            try {
                const response = await fetch(`${import.meta.env.VITE_API_URL}/api/quiz/${id}`); // Načítání dat kvízu
                if (!response.ok) {
                    throw new Error("Failed to fetch quiz"); // Chyba při načítání
                }
                const data: Quiz = await response.json(); // Získání dat ve formátu kvízu
                setQuiz(data); // Uložení kvízu do stavu
            } catch (error) {
                console.error("Error fetching quiz:", error); // Logování chyby
            } finally {
                setLoading(false); // Nastavení načítání na false po dokončení
            }
        };

        fetchQuiz(); // Zavolání funkce pro načtení kvízu
    }, [id]); // Efekt se spustí při změně ID kvízu v URL

    // Nastavení názvu stránky
    useEffect(() => {
        document.title = "View"; 
    }, []); 

    // Zobrazení načítání nebo chybové hlášky, pokud kvíz není nalezen
    if (loading) return <div>Loading...</div>; // Indikátor načítání
    if (!quiz) return <div>Quiz not found</div>; // Chybová hláška, pokud kvíz není nalezen

    return (
        <div className={styles.viewQuizPage}>
            <h1 className={styles.title}>{quiz.name}</h1> {/* Název kvízu */}
            <p className={styles.description}>{quiz.description}</p> {/* Popis kvízu */}
            <div className={styles.questionsContainer}>
                {quiz.questions.map((question, questionIndex) => (
                    <div key={question.id} className={styles.question}>
                        <h3 className={styles.questionTitle}>
                            {questionIndex + 1}. {question.text}
                        </h3> {/* Zobrazení textu otázky */}
                        <ul className={styles.answersList}>
                            {question.answers.map((answer) => (
                                <li
                                    key={answer.id}
                                    className={`${styles.answer} ${
                                        answer.isCorrect ? styles.correctAnswer : ""
                                    }`}
                                >
                                    {answer.text} {/* Zobrazení odpovědi */}
                                </li>
                            ))}
                        </ul>
                    </div>
                ))}
            </div>
            <button className={styles.backButton} onClick={() => navigate("/user")}>
                Back {/* Tlačítko pro návrat na stránku uživatele */}
            </button>
        </div>
    );
};

export default ViewQuizPage;
