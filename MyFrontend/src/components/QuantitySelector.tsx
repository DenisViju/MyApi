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
        <div>
            <span>Cantitate: </span>

            <button
                type="button"
                onClick={scadeCantitate}
                disabled={cantitate === 1}
            >
                -
            </button>

            <span> {cantitate} </span>

            <button
                type="button"
                onClick={cresteCantitate}
                disabled={cantitate === stoc}
            >
                +
            </button>
        </div>
    )
}

export default QuantitySelector
