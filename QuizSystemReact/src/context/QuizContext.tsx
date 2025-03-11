// src/context/QuizContext.js
import React, { createContext, ReactNode, useState, useEffect } from 'react';

// Definice typu pro QuizContext, který poskytuje metody a stav pro práci s kvízy
interface QuizContextType {
    quizzes: any[];  // Stav pro kvízy, které budou uloženy v poli
    setQuizzes: React.Dispatch<React.SetStateAction<any[]>>;  // Funkce pro aktualizaci seznamu kvízů
    fetchQuizzes: () => Promise<void>;  // Funkce pro získání všech kvízů
    fetchUserQuizzes: () => Promise<void>;  // Funkce pro získání kvízů vytvořených aktuálním uživatelem
}

// Počáteční stav pro QuizContext
const initialQuizContext: QuizContextType = {
    quizzes: [],
    setQuizzes: () => {},
    fetchQuizzes: async () => {},
    fetchUserQuizzes: async () => {},
};

// Vytvoření kontextu pro kvízy
export const QuizContext = createContext(initialQuizContext);

// Provider, který poskytuje kontext pro komponenty v aplikaci
export const QuizProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
    const [quizzes, setQuizzes] = useState<any[]>([]);  // Stav pro seznam kvízů

    // Funkce pro načtení všech kvízů
    const fetchQuizzes = async () => {
        try {
            // Poslání požadavku na server pro získání kvízů
            const response = await fetch(`${import.meta.env.VITE_API_URL}/api/Quiz`);
            console.log('API Response:', response);

            if (response.ok) {
                const data = await response.json();
                console.log('Fetched Quizzes:', data);
                setQuizzes(data);  // Nastavení kvízů do stavu
            } else {
                console.error("Failed to fetch quizzes, Status Code:", response.status);  // Výpis chyby při neúspěchu
            }
        } catch (error) {
            console.error("Fetch quizzes error:", error);  // Výpis chyby při výjimce
        }
    };

    // Funkce pro načtení kvízů vytvořených aktuálním uživatelem
    const fetchUserQuizzes = async () => {
        const token = localStorage.getItem('accessToken');  // Získání tokenu pro autentizaci
        try {
            // Poslání požadavku na server pro získání kvízů uživatele
            const response = await fetch(`${import.meta.env.VITE_API_URL}/api/quiz/my`, {
                method: 'GET',
                headers: {
                    'Authorization': `Bearer ${token}`,  // Poslání tokenu v hlavičce pro autentizaci
                },
            });

            if (response.ok) {
                const data = await response.json();
                setQuizzes(data);  // Nastavení kvízů uživatele do stavu
            } else {
                console.error("Failed to fetch user quizzes");  // Výpis chyby při neúspěchu
            }
        } catch (error) {
            console.error("Fetch user quizzes error:", error);  // Výpis chyby při výjimce
        }
    };

    // Načtení všech kvízů při prvním renderování komponenty
    useEffect(() => {
        fetchQuizzes();
    }, []);  // Prázdný seznam závislostí znamená, že efekt běží pouze jednou

    // Poskytnutí kontextu pro všechny komponenty uvnitř QuizProvider
    return (
        <QuizContext.Provider value={{ quizzes, setQuizzes, fetchQuizzes, fetchUserQuizzes }}>
            {children}
        </QuizContext.Provider>
    );
};
