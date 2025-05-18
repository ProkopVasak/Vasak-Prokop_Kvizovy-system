import React, { createContext, ReactNode, useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

interface AuthContextType {
    user: { name: string } | null;  
    login: (email: string, password: string) => Promise<void>; 
    register: (email: string, password: string) => Promise<void>;  
    logout: () => Promise<void>;  
    fetchUserInfo: () => Promise<void>; 
}

const initialAuthContext: AuthContextType = {
    user: null,
    login: async () => {},
    register: async () => {},
    logout: async () => {},
    fetchUserInfo: async () => {},
};

export const AuthContext = createContext(initialAuthContext);

export const AuthProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
    const [user, setUser] = useState<{ name: string } | null>(null); 
    const navigate = useNavigate();  

    const login = async (email: string, password: string) => {
  try {
    const response = await fetch(`${import.meta.env.VITE_API_URL}/api/login`, {
      credentials: 'include', // cookie budou odesílány a přijímány
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({ email, password }),
    });

    if (!response.ok) {
      throw new Error('Failed to login');
    }

    await fetchUserInfo();
    navigate('/');
  } catch (error) {
    console.error("Login error:", error);
  }
};

const fetchUserInfo = async () => {
        try {
            const response = await fetch(`${import.meta.env.VITE_API_URL}/api/manage/info`, {
    method: "GET",
    credentials: "include", // posíláme cookies spolu s požadavkem
    headers: {
      "Content-Type": "application/json",
    },
  });

            if (response.ok) {
                const userData = await response.json();
                setUser({ name: userData.email });
                console.log("Přihlášený uživatel:", userData);
            } else {
                setUser(null);
                console.error("Failed to fetch user info");
            }
        } catch (error) {
            setUser(null);
            console.error("Error fetching user info:", error);
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
        try {
            const response = await fetch(`${import.meta.env.VITE_API_URL}/api/logout`, {
                credentials: 'include',  
                method: 'POST',
                headers: {  // Poslání tokenu pro autentizaci
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify("logout"),  // Tělo požadavku pro odhlášení
            });

            if (!response.ok) {
                throw new Error('Failed to logout');
            }

            setUser(null);
            navigate('/login');  // Přesměrování na stránku přihlášení
        } catch (error) {
            console.error("Logout error:", error);  // Zachycení a výpis chyby
        }
    };

    // Načtení informací o uživateli při startu aplikace, pokud je uložený token
    useEffect(() => {
        fetchUserInfo();
    }, []);


    // Poskytnutí AuthContext pro všechny komponenty uvnitř AuthProvider
    return (
        <AuthContext.Provider value={{ user, login, register, logout, fetchUserInfo }}>
            {children}
        </AuthContext.Provider>
    );
};
