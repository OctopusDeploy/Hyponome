/// <reference path="_references.js" />
function GetPullRequests() {
    return $.get('https://api.github.com/repos/OctopusDeploy/Library/pulls', function (pulls) {
        console.log(pulls);
        return pulls;
    });
}
//$(document).ready(function () {
//    var editor = ace.edit('editor');
//    var files = [];
//    editor.getSession().setMode('ace/mode/diff');
//    editor.setTheme('ace/theme/github');
//    editor.getSession().setUseWrapMode(true);
//    $.get('https://api.github.com/repos/OctopusDeploy/Library/pulls/145/files', function (data) {
//        files = data;
//        editor.setValue(files[0].patch);
//    });
//});
