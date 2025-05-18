// src/components/QuizPage.tsx
import { useEffect, useState, useContext } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import styles from './QuizPage.module.css';
import { QuizContext } from '../context/QuizContext';

export interface Answer {
  id: string;
  text: string;
  isCorrect: boolean;
}

export interface Question {
  id: string;
  text: string;
  answers: Answer[];
}

export interface Quiz {
  id: string;
  name: string;
  description: string;
  questions: Question[];
}

export interface QuizResult {
  correctAnswers: number;
  totalQuestions: number;
}

const QuizPage = () => {
  const { id } = useParams(); // ID kvízu z URL
  const navigate = useNavigate();
  const { quizProgress, setQuizProgress } = useContext(QuizContext);

  const [quiz, setQuiz] = useState<Quiz | null>(null);
  const [loading, setLoading] = useState(true);
  const [currentQuestionIndex, setCurrentQuestionIndex] = useState(0);
  const [userAnswers, setUserAnswers] = useState<
    { text: string; answers: { text: string }[] }[]
  >([]);
  const [quizResult, setQuizResult] = useState<QuizResult | null>(null);

  // Načtení pokroku kvízu z QuizContextu
  useEffect(() => {
    if (id && quizProgress[id]) {
      setUserAnswers(quizProgress[id].savedAnswers);
      setCurrentQuestionIndex(quizProgress[id].savedIndex);
    }
  }, [id, quizProgress]);

  // Načítání kvízu z API
  useEffect(() => {
    const fetchQuiz = async () => {
      try {
        const response = await fetch(
          `${import.meta.env.VITE_API_URL}/api/quizzes/${id}`
        );
        if (!response.ok) {
          throw new Error('Failed to fetch quiz');
        }
        const data: Quiz = await response.json();
        setQuiz(data);
      } catch (error) {
        console.error('Error fetching quiz:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchQuiz();
  }, [id]);

  // Funkce pro vybrání odpovědi
  const handleAnswerSelect = (answerText: string) => {
    const currentQuestion = quiz?.questions[currentQuestionIndex];
    if (currentQuestion) {
      const updatedAnswers = [...userAnswers];
      updatedAnswers[currentQuestionIndex] = {
        text: currentQuestion.text,
        answers: [{ text: answerText }],
      };
      setUserAnswers(updatedAnswers);

      // Aktualizace pokroku v QuizContextu
      if (id) {
        setQuizProgress((prev) => ({
          ...prev,
          [id]: { savedAnswers: updatedAnswers, savedIndex: currentQuestionIndex },
        }));
      }
    }
  };

  // Funkce pro přechod na další otázku nebo odeslání kvízu
  const handleNextQuestion = () => {
    if (currentQuestionIndex < (quiz?.questions.length || 0) - 1) {
      const newIndex = currentQuestionIndex + 1;
      setCurrentQuestionIndex(newIndex);
      // Aktualizace pokroku v QuizContextu
      if (id) {
        setQuizProgress((prev) => ({
          ...prev,
          [id]: { savedAnswers: userAnswers, savedIndex: newIndex },
        }));
      }
    } else {
      submitQuiz();
    }
  };

  // Funkce pro odeslání kvízu a získání výsledků
  const submitQuiz = async () => {
    try {
      const response = await fetch(
        `${import.meta.env.VITE_API_URL}/api/quizzes/${id}/submissions`,
        {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify(userAnswers),
        }
      );

      if (!response.ok) {
        throw new Error('Failed to submit quiz');
      }

      const result: QuizResult = await response.json();
      setQuizResult(result);

      // Po dokončení kvízu smažeme pokrok pro daný kvíz z QuizContextu
      if (id) {
        setQuizProgress((prev) => {
          const updatedProgress = { ...prev };
          delete updatedProgress[id];
          return updatedProgress;
        });
      }
    } catch (error) {
      console.error('Error submitting quiz:', error);
    }
  };

  // Změna názvu stránky
  useEffect(() => {
    document.title = 'Quiz';
  }, []);

  if (loading) {
    return <div>Loading...</div>;
  }

  if (!quiz) {
    return <div>Quiz not found</div>;
  }

  if (quizResult) {
    return (
      <div className={styles.resultContainer}>
        <h2 className={styles.resultTitle}>Quiz Completed</h2>
        <p className={styles.resultMessage}>
          You answered {quizResult.correctAnswers} out of{' '}
          {quizResult.totalQuestions} questions correctly.
        </p>
        <button className={styles.homeButton} onClick={() => navigate('/')}>
          Home
        </button>
      </div>
    );
  }

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
        <button className={styles.nextButton} onClick={handleNextQuestion}>
          {currentQuestionIndex < quiz.questions.length - 1
            ? 'Next Question'
            : 'Submit Quiz'}
        </button>
      </div>
    </div>
  );
};

export default QuizPage;
