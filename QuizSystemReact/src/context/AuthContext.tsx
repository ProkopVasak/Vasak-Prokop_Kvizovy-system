import React, { createContext, ReactNode, useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

// Definice typu pro AuthContext, který poskytuje metody a stav pro práci s autentizací
interface AuthContextType {
    user: { name: string } | null;  // Informace o přihlášeném uživateli (pokud je přihlášen)
    login: (email: string, password: string) => Promise<void>;  // Funkce pro přihlášení
    register: (email: string, password: string) => Promise<void>;  // Funkce pro registraci
    logout: () => Promise<void>;  // Funkce pro odhlášení
    fetchUserInfo: () => Promise<void>;  // Funkce pro získání informací o uživateli
}

// Počáteční stav pro AuthContext, kdy je uživatel null a funkce jsou prázdné
const initialAuthContext: AuthContextType = {
    user: null,
    login: async () => {},
    register: async () => {},
    logout: async () => {},
    fetchUserInfo: async () => {},
};

// Vytvoření kontextu pro autentizaci
export const AuthContext = createContext(initialAuthContext);

// Provider, který poskytuje kontext pro komponenty v aplikaci
export const AuthProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
    const [user, setUser] = useState<{ name: string } | null>(null);  // Stav pro uživatele
    const navigate = useNavigate();  // Hook pro navigaci po přihlášení/odhlášení

    // Funkce pro přihlášení uživatele
    const login = async (email: string, password: string) => {
        try {
            // Poslání požadavku na přihlášení
            const response = await fetch(`${import.meta.env.VITE_API_URL}/api/login`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ email, password }),  // Odeslání emailu a hesla
            });

            // Zkontrolování odpovědi a uložení tokenu do localStorage
            if (!response.ok) {
                throw new Error('Failed to login');
            }
            const data = await response.json();
            localStorage.setItem('accessToken', data.accessToken);

            // Načtení informací o uživateli a přesměrování na hlavní stránku
            await fetchUserInfo();
            navigate('/');
        } catch (error) {
            console.error("Login error:", error);  // Zachycení a výpis chyby
        }
    };

    // Funkce pro získání informací o uživateli na základě uloženého tokenu
    const fetchUserInfo = async () => {
        const token = localStorage.getItem('accessToken');
        try {
            const response = await fetch(`${import.meta.env.VITE_API_URL}/api/manage/info`, {
                headers: {
                    'Authorization': `Bearer ${token}`,  // Poslání tokenu pro autentizaci
                },
            });

            if (response.ok) {
                const userData = await response.json();
                setUser({ name: userData.email });  // Nastavení informací o uživateli
            } else {
                console.error("Failed to fetch user info");
            }
        } catch (error) {
            console.error("Error fetching user info:", error);  // Zachycení a výpis chyby
        }
    };

    // Funkce pro registraci nového uživatele
    const register = async (email: string, password: string) => {
        try {
            // Poslání požadavku na registraci
            const response = await fetch(`${import.meta.env.VITE_API_URL}/api/register`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ email, password, secured: 'abcXYZ' }),  // Odeslání údajů o registraci
            });

            if (!response.ok) {
                throw new Error('Failed to register');
            }

            // Po úspěšné registraci se automaticky přihlásí
            await login(email, password);
        } catch (error) {
            console.error("Register error:", error);  // Zachycení a výpis chyby
        }
    };

    // Funkce pro odhlášení uživatele
    const logout = async () => {
        const token = localStorage.getItem('accessToken');
        try {
            const response = await fetch(`${import.meta.env.VITE_API_URL}/api/logout`, {
                method: 'POST',
                headers: {
                    'Authorization': `Bearer ${token}`,  // Poslání tokenu pro autentizaci
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify("logout"),  // Tělo požadavku pro odhlášení
            });

            if (!response.ok) {
                throw new Error('Failed to logout');
            }

            // Odstranění tokenu a nastavení stavu uživatele na null (odhlášení)
            localStorage.removeItem('accessToken');
            setUser(null);
            navigate('/login');  // Přesměrování na stránku přihlášení
        } catch (error) {
            console.error("Logout error:", error);  // Zachycení a výpis chyby
        }
    };

    // Načtení informací o uživateli při startu aplikace, pokud je uložený token
    useEffect(() => {
        if (localStorage.getItem('accessToken')) {
            fetchUserInfo();
        }
    }, []);

    // Poskytnutí AuthContext pro všechny komponenty uvnitř AuthProvider
    return (
        <AuthContext.Provider value={{ user, login, register, logout, fetchUserInfo }}>
            {children}
        </AuthContext.Provider>
    );
};
