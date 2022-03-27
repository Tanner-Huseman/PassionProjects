let isGameOver = false;
const pageTitle = 'QWIXX Score Card';

const redRow = document.getElementById('redUl');
const yellowRow = document.getElementById('yellowUl');
const blueRow = document.getElementById('blueUl');
const greenRow = document.getElementById('greenUl');

const redYellowRowValues = [
    { id: 2, isSelectable: true, wasSelected: false},
    { id: 3, isSelectable: true, wasSelected: false},
    { id: 4, isSelectable: true, wasSelected: false},
    { id: 5, isSelectable: true, wasSelected: false},
    { id: 6, isSelectable: true, wasSelected: false},
    { id: 7, isSelectable: true, wasSelected: false},
    { id: 8, isSelectable: true, wasSelected: false},
    { id: 9, isSelectable: true, wasSelected: false},
    { id: 10, isSelectable: true, wasSelected: false},
    { id: 11, isSelectable: true, wasSelected: false},
    { id: 12, isSelectable: true, wasSelected: false},
    { id: 13, isSelectable: false, wasSelected: false}
];

const greenBlueRowValues = [
    { id: 12, isSelectable: true, wasSelected: false},
    { id: 11, isSelectable: true, wasSelected: false},
    { id: 10, isSelectable: true, wasSelected: false},
    { id: 9, isSelectable: true, wasSelected: false},
    { id: 8, isSelectable: true, wasSelected: false},
    { id: 7, isSelectable: true, wasSelected: false},
    { id: 6, isSelectable: true, wasSelected: false},
    { id: 5, isSelectable: true, wasSelected: false},
    { id: 4, isSelectable: true, wasSelected: false},
    { id: 3, isSelectable: true, wasSelected: false},
    { id: 2, isSelectable: true, wasSelected: false},
    { id: 13, isSelectable: false, wasSelected: false}
];

function setPageTitle(){
    const title = document.getElementById('title');
    title.innerText = pageTitle;
}

function displayRows() {


    redYellowRowValues.forEach((numObject) => {
        const li = document.createElement('li');
        li.innerText = numObject.id;
        li.classList.add('red');
        li.classList.add('clickable');
        li.setAttribute('id', `r${numObject.id}`);
        redRow.insertAdjacentElement('beforeend', li);
    });   

    redYellowRowValues.forEach((numObject) => {
        const li = document.createElement('li');
        li.innerText = numObject.id;
        li.classList.add('yellow');
        li.classList.add('clickable');
        li.setAttribute('id', `y${numObject.id}`);
        yellowRow.insertAdjacentElement('beforeend', li);
    });

    greenBlueRowValues.forEach((numObject) => {
        const li = document.createElement('li');
        li.innerText = numObject.id;
        li.classList.add('green');
        li.classList.add('clickable');
        li.setAttribute('id', `g${numObject.id}`);
        greenRow.insertAdjacentElement('beforeend', li);
    });

    greenBlueRowValues.forEach((numObject) => {
        const li = document.createElement('li');
        li.innerText = numObject.id;
        li.classList.add('blue');
        li.classList.add('clickable');
        li.setAttribute('id', `b${numObject.id}`);
        blueRow.insertAdjacentElement('beforeend', li);
    });
};

function displayScore(score = 0){
    const scoreElement = document.getElementById('score');
    scoreElement.innerText = score;
}

function xCountToPointsConversion(xCount){
    let points;
    points = ((xCount * (xCount+1))/2);
    return points;
}

function calculateScore(){
    let score = 0;
    
    const rowElements = document.querySelectorAll('.rows div ul');
    rowElements.forEach( ul => {
        const listItems = ul.childNodes;
        let xCount = 0;
        listItems.forEach( element => {
            if(element.innerText == 'X'){
                xCount += 1;
            }           
        });
        score += xCountToPointsConversion(xCount);
    });
    displayScore(score);
};

const blockSelector = (event) => {
    let rowNum = 0;
    let rowColor;
    if (event.target.nodeName.toLowerCase()==='li' && event.target.classList.contains('clickable')){
        rowNum = parseInt(event.target.innerText);
        rowColor = (event.target.getAttribute('id')).charAt(0);
        event.target.innerText = 'X';
        event.target.style.backgroundColor = 'lightGray';
        lockoutLeft(rowColor, rowNum);
        calculateScore();
    };
};

function lockoutLeft(rowColor, rowNum){
    if(rowColor === 'r' || rowColor === 'y'){
        for(let i = 2; i < rowNum; i++){
            let lockoutBlock = document.getElementById(`${rowColor}${i}`);
            if(lockoutBlock.innerText !== 'X'){
                lockoutBlock.innerText = '_';
                lockoutBlock.style.backgroundColor = 'lightGray';
                lockoutBlock.classList.remove('clickable');
            };
        };
    }    
    else{
        for(let i = rowNum + 1; i < 13; i++){
            let lockoutBlock = document.getElementById(`${rowColor}${i}`);
            if(lockoutBlock.innerText !== 'X'){
                lockoutBlock.innerText = '_';
                lockoutBlock.style.backgroundColor = 'lightGray';
                lockoutBlock.classList.remove('clickable');
            };
        }; 
    }
};




document.addEventListener('DOMContentLoaded', ()=>{
    setPageTitle();

    displayRows();

    displayScore();

    const rows = document.querySelectorAll('.row');
    rows.forEach(row => {
        row.addEventListener('click', blockSelector);        
    });



});