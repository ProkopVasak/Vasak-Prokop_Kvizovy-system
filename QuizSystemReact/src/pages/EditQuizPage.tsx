import React, { useEffect, useState, useContext } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { Quiz } from './QuizPage';
import styles from './EditQuizPage.module.css';
import { QuizContext } from '../context/QuizContext';

const EditQuizPage = () => {
    // Získání id quizu z URL parametru
    const { id } = useParams();
    const navigate = useNavigate();

    // Stav pro uložení načteného quizu a loading stavu
    const [quiz, setQuiz] = useState<Quiz | null>(null);
    const [loading, setLoading] = useState(true);
    const { fetchQuizzes } = useContext(QuizContext);

    // Načítání quizu z API při inicializaci komponenty
    useEffect(() => {
        const fetchQuiz = async () => {
            try {
                const response = await fetch(`${import.meta.env.VITE_API_URL}/api/quiz/${id}`);
                if (!response.ok) {
                    throw new Error('Failed to fetch quiz');
                }
                const data = await response.json();
                setQuiz(data);
            } catch (error) {
                console.error('Error fetching quiz:', error);
            } finally {
                setLoading(false); // Nastavení loading stavu na false po načtení
            }
        };

        fetchQuiz();
    }, [id]); // Znovu načíst quiz pokud se změní id

    // Funkce pro změnu názvu quizu
    const handleQuizNameChange = (value: string) => {
        setQuiz((prevQuiz) => prevQuiz ? { ...prevQuiz, name: value } : null);
    };

    // Funkce pro změnu popisu quizu
    const handleQuizDescriptionChange = (value: string) => {
        setQuiz((prevQuiz) => prevQuiz ? { ...prevQuiz, description: value } : null);
    };

    // Funkce pro změnu textu otázky
    const handleQuestionTextChange = (questionIndex: number, value: string) => {
        if (!quiz) return; // Pokud není quiz načtený, neprovádět změny
        const updatedQuestions = [...quiz.questions];
        updatedQuestions[questionIndex].text = value;
        setQuiz((prevQuiz) => prevQuiz ? { ...prevQuiz, questions: updatedQuestions } : null);
    };

    // Funkce pro změnu textu odpovědi
    const handleAnswerTextChange = (questionIndex: number, answerIndex: number, value: string) => {
        if (!quiz) return; // Pokud není quiz načtený, neprovádět změny
        const updatedQuestions = [...quiz.questions];
        updatedQuestions[questionIndex].answers[answerIndex].text = value;
        setQuiz((prevQuiz) => prevQuiz ? { ...prevQuiz, questions: updatedQuestions } : null);
    };

    // Funkce pro změnu správnosti odpovědi
    const handleAnswerCorrectChange = (questionIndex: number, answerIndex: number, isCorrect: boolean) => {
        if (!quiz) return; // Pokud není quiz načtený, neprovádět změny
        const updatedQuestions = [...quiz.questions];
        updatedQuestions[questionIndex].answers[answerIndex].isCorrect = isCorrect;
        setQuiz((prevQuiz) => prevQuiz ? { ...prevQuiz, questions: updatedQuestions } : null);
    };

    // Funkce pro přidání nové otázky do quizu
    const handleAddQuestion = async () => {
        try {
            const quizId = quiz?.id; 
            if (!quizId) {
                console.error("Quiz ID is missing");
                return;
            }

            // Odeslání požadavku na API pro generování nové otázky
            const response = await fetch(`${import.meta.env.VITE_API_URL}/api/quiz/generate-question/${quizId}`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${localStorage.getItem("accessToken")}`, // Odeslání tokenu pro autentifikaci
                },
            });

            if (!response.ok) {
                throw new Error("Failed to generate question");
            }

            const newQuestion = await response.json();

            // Přidání nové otázky do stávajících otázek
            setQuiz((prevQuiz) => {
                if (!prevQuiz) return null;
                return {
                    ...prevQuiz,
                    questions: [...prevQuiz.questions, newQuestion],
                };
            });
        } catch (error) {
            console.error("Error adding question:", error); // Chyba při přidávání otázky
        }
    };

    // Funkce pro odstranění otázky
    const handleRemoveQuestion = (questionIndex: number) => {
        setQuiz((prevQuiz) => prevQuiz ? {
            ...prevQuiz,
            questions: prevQuiz.questions.filter((_, index) => index !== questionIndex)
        } : null); // Odstranění otázky z pole
    };

    // Funkce pro odeslání změn na server (PUT request)
    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
    
        if (!quiz) return; // Pokud není quiz načtený, nezpracovávat formulář
    
        const quizDto = {
            name: quiz.name,
            description: quiz.description,
            questions: quiz.questions.map((q) => ({
                id: q.id || null, 
                text: q.text,
                answers: q.answers.map((a) => ({
                    id: a.id || null, 
                    text: a.text,
                    isCorrect: a.isCorrect,
                })),
            })),
        };

        const token = localStorage.getItem('accessToken');
        if (!token) {
            // Pokud není token, přesměrovat na přihlašovací stránku
            navigate('/login');
            return;
        }

        try {
            const response = await fetch(`${import.meta.env.VITE_API_URL}/api/quiz/${id}`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                    Authorization: `Bearer ${token}`,
                },
                body: JSON.stringify(quizDto), // Odeslání dat ve formátu JSON
            });

            if (!response.ok) {
                throw new Error('Failed to update quiz');
            }
            await fetchQuizzes(); // Načíst kvízy znovu po úspěšné změně
            navigate(`/quiz/${id}`); // Přesměrování na stránku s upraveným kvízem
        } catch (error) {
            console.error('Error updating quiz:', error); // Chyba při odesílání dat
        }
    };

    // Funkce pro smazání kvízu
    const handleDelete = async () => {
        try {
            const token = localStorage.getItem('accessToken');
            const response = await fetch(`${import.meta.env.VITE_API_URL}/api/quiz/${id}`, {
                method: 'DELETE',
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            });

            if (!response.ok) {
                throw new Error('Failed to delete quiz');
            }

            navigate('/'); // Po smazání přesměrovat na hlavní stránku
        } catch (error) {
            console.error('Error deleting quiz:', error); // Chyba při mazání kvízu
        }
    };

    // Pokud se kvíz stále načítá, zobrazit zprávu o načítání
    if (loading) return <div>Loading...</div>;
    // Pokud kvíz nebyl nalezen, zobrazit zprávu
    if (!quiz) return <div>Quiz not found</div>;

    return (
        <div className={styles.editQuizPage}>
            <h1 className={styles.title}>Edit Quiz</h1>
            <form onSubmit={handleSubmit}>
                <div className={styles.formGroup}>
                    <label className={styles.label}>Name</label>
                    <input
                        className={styles.input}
                        type="text"
                        value={quiz.name}
                        onChange={(e) => handleQuizNameChange(e.target.value)}
                        maxLength={20}
                        required
                    />
                </div>
                <div className={styles.formGroup}>
                    <label className={styles.label}>Description</label>
                    <textarea
                        className={styles.textarea}
                        value={quiz.description}
                        onChange={(e) => handleQuizDescriptionChange(e.target.value)}
                        maxLength={100}
                        required
                    />
                </div>
                <h3>Questions</h3>
                {quiz.questions.map((question, questionIndex) => (
                    <div key={questionIndex} className={styles.questionContainer}>
                        <label className={styles.questionLabel}>Question {questionIndex + 1}</label>
                        <input
                            className={styles.input}
                            type="text"
                            value={question.text}
                            onChange={(e) => handleQuestionTextChange(questionIndex, e.target.value)}
                            maxLength={50}
                            required
                        />
                        <div>
                            <h4>Answers</h4>
                            {question.answers.map((answer, answerIndex) => (
                                <div key={answerIndex} className={styles.answerContainer}>
                                    <input
                                        className={styles.answerInput}
                                        type="text"
                                        value={answer.text}
                                        onChange={(e) => handleAnswerTextChange(questionIndex, answerIndex, e.target.value)}
                                        maxLength={20}
                                        required
                                    />
                                    <label className={styles.checkboxLabel}>
                                        Correct
                                        <input
                                            className={styles.checkbox}
                                            type="checkbox"
                                            checked={answer.isCorrect}
                                            onChange={(e) => handleAnswerCorrectChange(questionIndex, answerIndex, e.target.checked)}
                                        />
                                    </label>
                                </div>
                            ))}
                        </div>
                        <button
                            type="button"
                            className={styles.removeButton}
                            onClick={() => handleRemoveQuestion(questionIndex)}
                        >
                            Remove Question
                        </button>
                    </div>
                ))}
                <button type="button" className={styles.addButton} onClick={handleAddQuestion}>
                    Add Question
                </button>
                <div className={styles.buttonContainer}>
                    <button className={styles.saveButton} type="submit">Save Changes</button>
                    <button type="button" className={styles.deleteButton} onClick={handleDelete}>
                        Delete Quiz
                    </button>
                </div>
            </form>
        </div>
    );
};

export default EditQuizPage;
