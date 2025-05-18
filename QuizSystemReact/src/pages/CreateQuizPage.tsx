// src/pages/CreateQuizPage.js
import { useState, useContext, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import styles from './CreateQuizPage.module.css';
import { QuizContext } from '../context/QuizContext';

const CreateQuizPage = () => {
    // Stav pro název, popis a otázky kvízu
    const [name, setName] = useState('');
    const [description, setDescription] = useState('');
    const [questions, setQuestions] = useState([
        { text: '', answers: Array(4).fill(0).map(() => ({ text: '', isCorrect: false })) },  // Inicializace 1 otázky se 4 odpověďmi
    ]);
    
    const navigate = useNavigate();
    const { fetchQuizzes } = useContext(QuizContext);  // Funkce pro načítání kvízů

    // Funkce pro přidání nové otázky
    const handleAddQuestion = () => {
        setQuestions([
            ...questions,
            { text: '', answers: Array(4).fill(0).map(() => ({ text: '', isCorrect: false })) },  // Přidání nové otázky se 4 prázdnými odpověďmi
        ]);
    };

    // Funkce pro změnu textu otázky
    const handleQuestionChange = (index: number, value: string) => {
        const updatedQuestions = [...questions];
        updatedQuestions[index].text = value;
        setQuestions(updatedQuestions);
    };

    // Funkce pro změnu textu odpovědi
    const handleAnswerTextChange = (
        questionIndex: number,
        answerIndex: number,
        newText: string
    ) => {
        const updatedQuestions = [...questions];
        updatedQuestions[questionIndex].answers[answerIndex].text = newText;
        setQuestions(updatedQuestions);
    };

    const handleAnswerCorrectChange = (
        questionIndex: number,
        answerIndex: number
            ) => {
                setQuestions(prev =>
                    prev.map((q, qi) => {
                        if (qi !== questionIndex) return q;
                            return {
                                ...q,
                                answers: q.answers.map((a, ai) => ({
                                ...a,
                                isCorrect: ai === answerIndex,  // jedině toto je true
                })),
            };
            })
        );
    };


    // Funkce pro odeslání formuláře pro vytvoření kvízu
    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();  // Zamezí odeslání formuláře a obnovování stránky

        // Vytvoření DTO pro odeslání na server
        const quizDto = {
            name,
            description,
            questions: questions.map((q) => ({
                text: q.text,
                answers: q.answers.map((a) => ({
                    text: a.text,
                    isCorrect: a.isCorrect,
                })),
            })),
        };


        try {
            // Odeslání POST požadavku pro vytvoření kvízu
            const response = await fetch(`${import.meta.env.VITE_API_URL}/api/quizzes`, {
                credentials: 'include',  
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(quizDto),
            });

            if (!response.ok) {
                throw new Error('Failed to create quiz');
            }
            await fetchQuizzes();  // Načtení všech kvízů po vytvoření nového
            navigate(`/user/`);  // Přesměrování na stránku uživatele
        } catch (error) {
            console.error('Error creating quiz:', error);
        }
    };


    // Nastavení titulku stránky
    useEffect(() => {
        document.title = "Create";  // Titulek pro tuto stránku
    }, []);

    return (
        <div className={styles.createQuizPage}>
            <h1 className={styles.title}>Create a New Quiz</h1>
            <form onSubmit={handleSubmit}>
                {/* Sekce pro zadání názvu kvízu */}
                <div className={styles.formGroup}>
                    <label className={styles.label}>Name</label>
                    <input
                        className={styles.input}
                        type="text"
                        value={name}
                        onChange={(e) => setName(e.target.value)}  // Aktualizace názvu
                        maxLength={100}  // Max délka názvu kvízu
                        required
                    />
                </div>
                {/* Sekce pro zadání popisu kvízu */}
                <div className={styles.formGroup}>
                    <label className={styles.label}>Description</label>
                    <textarea
                        className={styles.textarea}
                        value={description}
                        onChange={(e) => setDescription(e.target.value)}  // Aktualizace popisu
                        maxLength={100}  // Max délka popisu
                        required
                    />
                </div>
                {/* Sekce pro přidání a správu otázek */}
                <div>
                    <h3 className={styles.title}>Questions</h3>
                    {questions.map((question, questionIndex) => (
                        <div key={questionIndex} className={styles.questionContainer}>
                            <label className={styles.questionLabel}>
                                Question {questionIndex + 1}
                            </label>
                            <input
                                className={styles.input}
                                type="text"
                                value={question.text}
                                maxLength={100}  // Max délka otázky
                                onChange={(e) =>
                                    handleQuestionChange(questionIndex, e.target.value)  // Změna textu otázky
                                }
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
                                        onChange={e =>
                                            handleAnswerTextChange(
                                            questionIndex,
                                            answerIndex,
                                            e.target.value
                                            )
                                        }
                                        required
                                        />
                                        <label className={styles.checkboxLabel}>
                                        Correct
                                        <input
                                            className={styles.checkbox}
                                            type="checkbox"
                                            checked={answer.isCorrect}
                                            onChange={() =>
                                            handleAnswerCorrectChange(
                                                questionIndex,
                                                answerIndex
                                            )
                                            }
                                        />
                                        </label>
                                    </div>
                                    ))}

                            </div>
                        </div>
                    ))}
                    {/* Tlačítko pro přidání nové otázky */}
                    <button
                        type="button"
                        className={styles.addButton}
                        onClick={handleAddQuestion}
                    >
                        Add Question
                    </button>
                </div>
                {/* Tlačítko pro odeslání formuláře a vytvoření kvízu */}
                <button className={styles.saveButton} type="submit">
                    Create Quiz
                </button>
            </form>
        </div>
    );
};

export default CreateQuizPage;