window.ticTacAnimation = {
    init: function (element, dotNetHelper) {
        this.boardElement = element;
        this.dotNetHelper = dotNetHelper;
    },

    positionWinLine: function (boardElement, firstCellIndex, lastCellIndex, length) {
        const board = boardElement;
        const cells = board.children;

        const firstCell = cells[firstCellIndex].getBoundingClientRect();
        const lastCell = cells[lastCellIndex].getBoundingClientRect();
        const boardRect = board.getBoundingClientRect();
        
        let winLine = board.querySelector('.win-line-animation');
        if (!winLine) {
            winLine = document.createElement('div');
            winLine.className = 'win-line-animation';
            board.appendChild(winLine);
        }
        
        winLine.style.width = `${lastCell.right - firstCell.left}px`;
        winLine.style.height = '10px';
        winLine.style.left = `${firstCell.left - boardRect.left}px`;
        winLine.style.top = `${firstCell.top + firstCell.height / 2 - boardRect.top - 5}px`;
        winLine.style.transform = 'scaleX(0)';
        winLine.style.transformOrigin = 'left center';
        
        setTimeout(() => {
            winLine.style.transition = 'transform 0.8s ease-out';
            winLine.style.transform = 'scaleX(1)';
        }, 10);
    },

    dispose: function() {
        if (this.boardElement) {
            const line = this.boardElement.querySelector('.win-line-animation');
            if (line) line.remove();
        }
        this.boardElement = null;
        this.dotNetHelper = null;
    }
};
