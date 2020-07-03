function addLis() {
    var xlf = document.getElementById('xlf');
   
    if (xlf.addEventListener) xlf.addEventListener('change', handleFile, false);
}



function handleDragover(e) {
    e.stopPropagation();
    e.preventDefault();
    e.dataTransfer.dropEffect = 'copy';
}

function onDropDown(e) {
    e.stopPropagation();
    e.preventDefault();
    var files = e.dataTransfer.files;
    var f = files[0];
    readFile(f);
}

function handleFile(e) {
    var files = e.target.files;
    var f = files[0];
    readFile(f);
}

function readFile(file) {

    //var name = file.name;
    //var reader = new FileReader();
    //reader.onload = function (e) {
    //    var data = e.target.result;
    //    var wb = XLSX.read(data, { type: "binary" });
    //    console.log(wb);
    //};
    //reader.readAsBinaryString(file);
}