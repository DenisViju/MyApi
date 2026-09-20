import { Navigate, Outlet } from "react-router-dom";
import { useAuth } from "../context/AuthContext";

function ProtectedRoute() {
    const {accessToken, isLoading} = useAuth()

    if(isLoading) {
        return <p>Se verifica autentificarea</p>
    }
    
    if(!accessToken) {
        return <Navigate to="/login" replace />
    }

    return <Outlet />
}

export default ProtectedRoute