// src/context/QuizContext.js
import React, { createContext, ReactNode, useState, useEffect } from 'react';

interface QuizProgressData {
  savedAnswers: { text: string; answers: { text: string }[] }[];
  savedIndex: number;
}

interface QuizContextType {
  userQuizzes: any[]; // Kvízy vytvořené uživateli
  quizzes: any[];     // Všechny kvízy
  setQuizzes: React.Dispatch<React.SetStateAction<any[]>>;
  fetchQuizzes: () => Promise<void>;
  fetchUserQuizzes: () => Promise<void>;
  // Nově přidáme stav pro pokrok v kvízech
  quizProgress: Record<string, QuizProgressData>;
  setQuizProgress: React.Dispatch<
    React.SetStateAction<Record<string, QuizProgressData>>
  >;
}

// Definice typu pro QuizContext, který poskytuje metody a stav pro práci s kvízy
interface QuizContextType {
    userQuizzes: any[];  // Stav pro kvízy vytvořené uživateli
    quizzes: any[];  // Stav pro kvízy, které budou uloženy v poli
    setQuizzes: React.Dispatch<React.SetStateAction<any[]>>;  // Funkce pro aktualizaci seznamu kvízů
    fetchQuizzes: () => Promise<void>;  // Funkce pro získání všech kvízů
    fetchUserQuizzes: () => Promise<void>;  // Funkce pro získání kvízů vytvořených aktuálním uživatelem
}

// Počáteční stav pro QuizContext
const initialQuizContext: QuizContextType = {
  userQuizzes: [],
  quizzes: [],
  setQuizzes: () => {},
  fetchQuizzes: async () => {},
  fetchUserQuizzes: async () => {},
  quizProgress: {},
  setQuizProgress: () => {},
};

// Vytvoření kontextu pro kvízy
export const QuizContext = createContext(initialQuizContext);

// Provider, který poskytuje kontext pro komponenty v aplikaci
export const QuizProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
    const [quizzes, setQuizzes] = useState<any[]>([]);  // Stav pro seznam kvízů
    const [userQuizzes, setUserQuizzes] = useState<any[]>([]);  // Stav pro seznam kvízů
    const [quizProgress, setQuizProgress] = useState<
        Record<string, QuizProgressData>
    >({});
    // Funkce pro načtení všech kvízů
    const fetchQuizzes = async () => {
        try {
            const response = await fetch(`${import.meta.env.VITE_API_URL}/api/quizzes`);
            console.log('API Response:', response);

            if (response.ok) {
                const data = await response.json();
                console.log('Fetched Quizzes:', data);
                setQuizzes(data);  
            } else {
                console.error("Failed to fetch quizzes, Status Code:", response.status);  
            }
        } catch (error) {
            console.error("Fetch quizzes error:", error);  
        }
    };

    
    // Funkce pro načtení kvízů vytvořených aktuálním uživatelem
    const fetchUserQuizzes = async () => {
        try {
            // Poslání požadavku na server pro získání kvízů uživatele
            const response = await fetch(`${import.meta.env.VITE_API_URL}/api/quizzes/mine`, {
                credentials: 'include',  
                method: 'GET',
                headers: {
                   
                },
            });

            if (response.ok) {
                const data = await response.json();
                setUserQuizzes(data);  // Nastavení kvízů uživatele do stavu
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
        <QuizContext.Provider value={{ userQuizzes, quizzes, setQuizzes, fetchQuizzes, fetchUserQuizzes, quizProgress, setQuizProgress }}>
            {children}
        </QuizContext.Provider>
    );
};
