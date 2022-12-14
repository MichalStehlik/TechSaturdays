import { Outlet, Link } from "react-router-dom"
import { useNavigate } from "react-router-dom"

export const AccountLayout = () => {
    return (
        <>
        <header>
            <menu>
                <li><Link to="/">Home</Link></li>
            </menu>
        </header>
        <main>
            <Outlet />
        </main>
        </>
    
    );
};

export default AccountLayout;