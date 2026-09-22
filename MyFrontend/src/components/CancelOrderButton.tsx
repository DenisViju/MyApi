import { useState } from "react";
import { useAuth } from "../context/AuthContext";
import { cancelOrder } from "../api/OrdersApi";
import type { Comanda } from "../types/Comanda";
import { comandaPoateFiAnulata } from "../utils/ComandaUtils";

type Props = {
    order: Comanda;
    onCancelled?: (order: Comanda) => void;
};

function CancelOrderButton({ order, onCancelled }: Props) {
    const { accessToken } = useAuth();
    const [isCancelling, setIsCancelling] = useState(false);
    const [error, setError] = useState<string | null>(null);
    console.log("STATUS:", order.status);
    console.log("POATE FI ANULATA:", comandaPoateFiAnulata(order));

    if (!comandaPoateFiAnulata(order)) {
        return null;
    }

    async function handleAnuleaza() {
        if (!accessToken) {
            setError("Nu esti autentificat");
            return;
        }

        const confirmat = window.confirm(
            "Esti sigur ca vrei sa anulezi comanda?"
        );

        if (!confirmat) {
            return;
        }

        try {
            setIsCancelling(true);
            setError(null);

            const comandaAnulata = await cancelOrder(
                order.id,
                accessToken
            );

            onCancelled?.(comandaAnulata);
        } catch (error: unknown) {
            if (error instanceof Error) {
                setError(error.message);
            } else {
                setError("A aparut o eroare necunoscuta");
            }
        } finally {
            setIsCancelling(false);
        }
    }

    return (
        <div>
            <button
                type="button"
                onClick={handleAnuleaza}
                disabled={isCancelling}
            >
                {isCancelling
                    ? "Se anuleaza.."
                    : "Anuleaza comanda"}
            </button>

            {error && <p>{error}</p>}
        </div>
    );
}

export default CancelOrderButton;