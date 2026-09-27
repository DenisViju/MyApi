type QuantitySelectorProps = {
    cantitate: number
    stoc: number
    onChange: (cantitate: number) => void
}

function QuantitySelector({
    cantitate,
    stoc,
    onChange
}: QuantitySelectorProps) {

    function scadeCantitate() {
        if (cantitate > 1) {
            onChange(cantitate - 1)
        }
    }

    function cresteCantitate() {
        if (cantitate < stoc) {
            onChange(cantitate + 1)
        }
    }

    return (
        <div className="quantity-selector">
            
            <button
                type="button"
                onClick={scadeCantitate}
                disabled={cantitate === 1}
                className="quantity-button"
            >
                -
            </button>

            <span className="quantity-value">
                {cantitate} 
            </span>

            <button
                type="button"
                onClick={cresteCantitate}
                disabled={cantitate === stoc}
                className="quantity-button"
            >
                +
            </button>
        </div>
    )
}

export default QuantitySelector
