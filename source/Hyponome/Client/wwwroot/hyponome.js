window.hyponome = {
    ace: {
        editor: (element, mode) => {
            const editor = ace.edit(element);
            editor.getSession().setMode(`ace/mode/${mode}`);
            editor.setTheme('ace/theme/github');
            editor.getSession().setUseWrapMode(false);
            editor.setReadOnly(true);
            editor.setOptions({ maxLines: Infinity });
        },
        diff: (element, leftContent, rightContent, mode) => {
            const aceDiff = new AceDiff({
                element: element,
                mode: `ace/mode/${mode}`,
                theme: 'ace/theme/github',
                showConnectors: false,
                left: {
                    content: leftContent,
                    editable: false,
                    copyLinkEnabled: false
                },
                right: {
                    content: rightContent,
                    editable: false,
                    copyLinkEnabled: false
                }
            });
        }
    },
    bootstrap: {
        tooltip: () => {
            $('[data-toggle="tooltip"]').tooltip();
        }
    },
    moment: {
        fromNow: (time) => {
            return moment(time).fromNow();
        }
    }
};