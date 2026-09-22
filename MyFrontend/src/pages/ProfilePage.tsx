import { Link } from "react-router-dom";
import { useAuth } from "../context/AuthContext";

function ProfilePage() {
    const { user } = useAuth()

    return(
        <div>
            <h1>Profilul meu</h1>

            {user && (
                <p>
                     Nume utilizator: {user.username} — Rol: {user.role}
                </p>
            )}

            <nav>
                <ul>
                    <li>
                        <Link to="/addresses">Adresele mele</Link>
                    </li>
                    <li>
                        <Link to="/orders">Comenzile mele</Link>
                    </li>
                </ul>
            </nav>
        </div>
    )
}

export default ProfilePage