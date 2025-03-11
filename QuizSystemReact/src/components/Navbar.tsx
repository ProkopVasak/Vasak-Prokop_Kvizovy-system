import { useState, useContext } from 'react';
import { AuthContext } from '../context/AuthContext';
import { useNavigate } from 'react-router-dom';
import styles from './Navbar.module.css';

const Navbar = () => {
    const { user, logout } = useContext(AuthContext);
    const navigate = useNavigate();
    const [isMenuOpen, setIsMenuOpen] = useState(false);

    const toggleMenu = () => {
        setIsMenuOpen((prev) => !prev);
    };

    return (
        <nav className={styles.navbar}>
            <div className={styles.logo} onClick={() => navigate('/')}>Kvízový systém</div>
            <div className={`${styles.hamburger} ${isMenuOpen ? styles.hamburgerOpen : ''}`} onClick={toggleMenu}>
                <div className={`${styles.line} ${isMenuOpen ? styles.lineTop : ''}`}></div>
                <div className={`${styles.line} ${isMenuOpen ? styles.lineMiddle : ''}`}></div>
                <div className={`${styles.line} ${isMenuOpen ? styles.lineBottom : ''}`}></div>
            </div>
            <div className={`${styles.menu} ${isMenuOpen ? styles.menuOpen : ''}`}>
                {user ? (
                    <>
                        <span className={styles.menuItem} onClick={() => navigate('/user')}>{user.name}</span>
                        <button className={styles.menuButton} onClick={logout}>Logout</button>
                    </>
                ) : (
                    <>
                        <button className={styles.menuButton} onClick={() => navigate('/login')}>Login</button>
                        <button className={styles.menuButton} onClick={() => navigate('/register')}>Register</button>
                    </>
                )}
            </div>
        </nav>
    );
};

export default Navbar;