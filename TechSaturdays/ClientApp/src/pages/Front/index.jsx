import { Outlet, Link } from "react-router-dom"
import { useAuthContext } from "../../providers/AuthProvider";

export const FrontLayout = () => {
    const [{accessToken}] = useAuthContext();
    return (
        <>
        <header>
            <menu>
                <li><a href="/swagger">Swagger</a></li>
                <li><Link to="/users">Users</Link></li>
                {!accessToken ? <li><Link to="/account/sign-in">Sign In</Link></li> : null}
                {!accessToken ? <li><Link to="/account/sign-up">Sign Up</Link></li> : null }
            </menu>
        </header>
        <main>
            <Outlet />
        </main>
        </>
    
    );
};

export default FrontLayout;