const movableGrid = document.querySelector(".movable-grid");
const handlerText = document.querySelector('.handler-text');
const cells = document.querySelectorAll('.game-cell');
const moveButtons = document.querySelectorAll('.move-btn')
let movingChip = false;
let chipPosition = [0, 0];

const connection = new signalR.HubConnectionBuilder()
    .withUrl("/gameHub")
    .build();

connection.start()
    .then(() => {
        console.log("Connected to SignalR!");

        connection.invoke("JoinGame");
    })
    .catch(err => console.error(err.toString()));

connection.on("GameStateUpdated", (gameState) => {
    console.log("Game state updated:", gameState);
    updateGameBoard(gameState);
});
connection.on("PlayerJoined", (playerId) => {
    console.log("Player joined:", playerId);
});

function makeMove(x, y) {
    connection.invoke("MakeMove", x, y)
        .catch(err => console.error(err.toString()));
}

function checkTurn(isYourTurn) {
    if (isYourTurn) {
        enableAllButtons();
        handlerText.innerText = "It's your turn!";
    } else {
        disableAllButtons();
        handlerText.innerText = "Wait for your opponent's move.";
    }
}


async function fetchGameState() {
    try {
        const response = await fetch('/GameOnline?handler=GameState', {
            method: 'GET',
            headers: { 'Content-Type': 'application/json' }
        });
        const data = await response.json();
        console.log(data)
        updateBoard(data.board);
        updateMovableGrid(data.gridX, data.gridY);
        showOptions(playerNumber)
        checkTurn(data.isYourTurn)

        if (data.win !== 0) {
            checkForWin(data.win);
        }
    } catch (error) {
        console.error('Error fetching game state:', error);
    }
}

setInterval(fetchGameState, 2000)

async function handleCellClick(x, y, buttonElement) {
    const number = +buttonElement.dataset.number;

    if (number === playerNumber) {
        movingChip = true;
        chipPosition = [x, y];
        buttonElement.dataset.number = 0;
        handlerText.innerText = "Move chip somewhere";
    } else if (movingChip) {
        const data = await fetchWithHandler('/GameOnline?handler=MoveChip', { startX: chipPosition[0], startY: chipPosition[1], endX: x, endY: y});
        movingChip = false;
        chipPosition = [0, 0];
        buttonElement.dataset.number = data.board[x][y];
        handlerText.innerText = data.message;
        updateBoard(data.board);
    } else {
        const data = await fetchWithHandler('/GameOnline?handler=Click', { x: x, y: y});
        buttonElement.dataset.number = data.board[x][y];
        buttonElement.innerText = data.board[x][y] === 1 ? 'X' : data.board[x][y] === 2 ? 'O' : '';
    }
}

async function handleMoveBoard(direction) {
    const data = await fetchWithHandler('/GameOnline?handler=MoveBoard', { direction: direction });
    updateMovableGrid(data.gridX, data.gridY);

    if (data.message) {
        handlerText.innerText = data.message;
    }
}

async function fetchWithHandler(url, body) {
    try {
        const response = await fetch(url, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(body)
        });
        if (!response.ok) throw new Error(`Error ${response.status}: ${response.statusText}`);
        const data = await response.json();
        playerNumber = data.playerNumber;
        gridX = data.gridX;
        gridY = data.gridY;
        showOptions(playerNumber);
        if (data.message) {
            handlerText.innerText = data.message;
        }
        checkTurn(data.isYourTurn)
        checkForWin(data.win);
        return data;
    } catch (error) {
        console.error('Fetch error:', error);
        alert("Something went wrong, please try again.");
    }
}

function updateBoard(board) {
    cells.forEach(cell => {
        const x = +cell.dataset.x;
        const y = +cell.dataset.y;
        cell.innerText = board[x][y] === 1 ? 'X' : board[x][y] === 2 ? 'O' : '';
    });
}

function updateMovableGrid(gridX, gridY) {
    movableGrid.style.left = `${gridX * 80}px`;
    movableGrid.style.top = `${gridY * 80}px`;

    cells.forEach(cell => {
        const x = +cell.dataset.x;
        const y = +cell.dataset.y;
        const isWithinGrid = x >= gridY && x < gridY + 3 && y >= gridX && y < gridX + 3;
        cell.disabled = !isWithinGrid;
    });
}

function checkForWin(win) {
    if (win === 1) {
        handlerText.innerText = "Player 1 wins!";
        disableAllButtons();
    } else if (win === 2) {
        handlerText.innerText = "Player 2 wins!";
        disableAllButtons();
    } else if (win === 3) {
        handlerText.innerText = "It's a draw!";
        disableAllButtons();
    }
}

function disableAllButtons() {
    disableAllCells();
    disableMoveButtons();
}

function enableAllButtons() {
    enableAllCells()
    enableMoveButtons()
}

function enableAllCells(){
    cells.forEach(cell => {
        cell.disabled = false;
    });
}

function disableAllCells(){
    cells.forEach(cell => {
        cell.disabled = true;
    });
}

function enableMoveButtons() {
    moveButtons.forEach(btn => {
        btn.disabled = false;
    })
}

function disableMoveButtons() {
    moveButtons.forEach(btn => {
        btn.disabled = true;
    })
}


function showOptions(player1Options, player2Options) {
    if (player1Options || player2Options) {
        enableMoveButtons()
    } else {
        disableMoveButtons()
    }
}