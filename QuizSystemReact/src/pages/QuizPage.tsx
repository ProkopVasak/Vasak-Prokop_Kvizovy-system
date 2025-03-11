import { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom'; // Hooky pro získání ID kvízu z URL a navigaci mezi stránkami
import styles from './QuizPage.module.css';

export interface Answer {
    id: string;
    text: string;
    isCorrect: boolean;
}

export interface Question {
    id: string;
    text: string;
    answers: Answer[]; // Možné odpovědi na otázku
}

export interface Quiz {
    id: string;
    name: string;
    description: string;
    questions: Question[]; // Seznam otázek kvízu
}

interface QuizResult {
    correctAnswers: number;
    totalQuestions: number; // Výsledky kvízu (kolik správných odpovědí)
}

const QuizPage = () => {
    const { id } = useParams(); // Získání ID kvízu z URL
    const navigate = useNavigate(); // Hook pro navigaci na jinou stránku
    const [quiz, setQuiz] = useState<Quiz | null>(null); // Stav pro uchování kvízu
    const [loading, setLoading] = useState(true); // Stav pro indikaci načítání
    const [currentQuestionIndex, setCurrentQuestionIndex] = useState(0); // Index aktuální otázky
    const [userAnswers, setUserAnswers] = useState<{ text: string; answers: { text: string }[] }[]>([]); // Uživatelské odpovědi
    const [quizResult, setQuizResult] = useState<QuizResult | null>(null); // Výsledky kvízu

    // Načtení pokroku kvízu z localStorage
    useEffect(() => {
        const savedQuizProgress = localStorage.getItem(`quiz-progress-${id}`);
        if (savedQuizProgress) {
            const { savedAnswers, savedIndex } = JSON.parse(savedQuizProgress);
            setUserAnswers(savedAnswers || []);
            setCurrentQuestionIndex(savedIndex || 0);
        }
    }, [id]);

    // Načítání kvízu z API
    useEffect(() => {
        const fetchQuiz = async () => {
            try {
                const response = await fetch(`${import.meta.env.VITE_API_URL}/api/quiz/${id}`);
                if (!response.ok) {
                    throw new Error('Failed to fetch quiz');
                }
                const data: Quiz = await response.json();
                setQuiz(data); // Nastavení kvízu do stavu
                localStorage.setItem('selectedQuiz', JSON.stringify(data)); // Uložení kvízu do localStorage
            } catch (error) {
                console.error("Error fetching quiz:", error);
            } finally {
                setLoading(false); // Ukončení načítání
            }
        };

        fetchQuiz();
    }, [id]);

    // Funkce pro výběr odpovědi
    const handleAnswerSelect = (answerText: string) => {
        const currentQuestion = quiz?.questions[currentQuestionIndex];
        if (currentQuestion) {
            const updatedAnswers = [...userAnswers];
            updatedAnswers[currentQuestionIndex] = { text: currentQuestion.text, answers: [{ text: answerText }] };
            setUserAnswers(updatedAnswers); // Aktualizace odpovědí

            // Uložení pokroku do localStorage
            localStorage.setItem(`quiz-progress-${id}`, JSON.stringify({
                savedAnswers: updatedAnswers,
                savedIndex: currentQuestionIndex,
            }));
        }
    };

    // Funkce pro přechod na další otázku nebo odeslání kvízu
    const handleNextQuestion = () => {
        if (currentQuestionIndex < (quiz?.questions.length || 0) - 1) {
            const newIndex = currentQuestionIndex + 1;
            setCurrentQuestionIndex(newIndex);

            // Uložení pokroku do localStorage
            localStorage.setItem(`quiz-progress-${id}`, JSON.stringify({
                savedAnswers: userAnswers,
                savedIndex: newIndex,
            }));
        } else {
            submitQuiz(); // Pokud je to poslední otázka, odeslání kvízu
        }
    };

    // Funkce pro odeslání kvízu a získání výsledků
    const submitQuiz = async () => {
        try {
            const response = await fetch(`${import.meta.env.VITE_API_URL}/api/quiz/${id}/submit`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(userAnswers), // Odeslání odpovědí
            });

            if (!response.ok) {
                throw new Error('Failed to submit quiz');
            }

            const result: QuizResult = await response.json();
            setQuizResult(result); // Nastavení výsledků

            // Smazání pokroku z localStorage po dokončení kvízu
            localStorage.removeItem(`quiz-progress-${id}`);
        } catch (error) {
            console.error("Error submitting quiz:", error);
        }
    };

    // Změna názvu stránky na "Quiz"
    useEffect(() => {
        document.title = "Quiz"; 
    }, []);

    // Zobrazujeme loading stav během načítání kvízu
    if (loading) {
        return <div>Loading...</div>;
    }

    // Pokud kvíz nebyl nalezen, zobrazíme zprávu
    if (!quiz) {
        return <div>Quiz not found</div>;
    }

    // Zobrazení výsledků kvízu po jeho odeslání
    if (quizResult) {
        return (
            <div className={styles.resultContainer}>
                <h2 className={styles.resultTitle}>Quiz Completed</h2>
                <p className={styles.resultMessage}>
                    You answered {quizResult.correctAnswers} out of {quizResult.totalQuestions} questions correctly.
                </p>
                <button className={styles.homeButton} onClick={() => navigate('/')}>
                    Go to Homepage
                </button>
            </div>
        );
    }

    // Získání aktuální otázky
    const currentQuestion = quiz.questions[currentQuestionIndex];

    return (
        <div className={styles.quizPage}>
            <h1 className={styles.title}>{quiz.name}</h1>
            <p className={styles.description}>{quiz.description}</p>
            <div className={styles.questionContainer}>
                <h3 className={styles.questionText}>{currentQuestion.text}</h3>
                <ul className={styles.answerList}>
                    {currentQuestion.answers.map((answer) => (
                        <li key={answer.id} className={styles.answerItem}>
                            <button
                                className={styles.answerButton}
                                onClick={() => handleAnswerSelect(answer.text)}
                            >
                                {answer.text}
                            </button>
                        </li>
                    ))}
                </ul>
                <button
                    className={styles.nextButton}
                    onClick={handleNextQuestion}
                >
                    {currentQuestionIndex < quiz.questions.length - 1 ? "Next Question" : "Submit Quiz"}
                </button>
            </div>
        </div>
    );
};

export default QuizPage;
