window.journalEditor = {
    wrapSelection: function (textareaId, left, right) {
        const ta = document.getElementById(textareaId);
        if (!ta) return null;

        ta.focus();

        const start = ta.selectionStart ?? 0;
        const end = ta.selectionEnd ?? 0;

        const before = ta.value.substring(0, start);
        const selected = ta.value.substring(start, end);
        const after = ta.value.substring(end);

        // If nothing selected, put cursor between wrappers
        const newSelected = selected.length === 0 ? "" : selected;
        const newValue = before + left + newSelected + right + after;

        ta.value = newValue;

        // cursor position
        const cursorStart = before.length + left.length;
        const cursorEnd = cursorStart + newSelected.length;

        ta.setSelectionRange(cursorStart, cursorEnd);
        return newValue;
    },

    insertLine: function (textareaId, text) {
        const ta = document.getElementById(textareaId);
        if (!ta) return null;

        ta.focus();

        const start = ta.selectionStart ?? 0;
        const before = ta.value.substring(0, start);
        const after = ta.value.substring(start);

        const prefix = before.length > 0 && !before.endsWith("\n") ? "\n" : "";
        const suffix = after.length > 0 && !after.startsWith("\n") ? "\n" : "\n";

        const newValue = before + prefix + text + suffix + after;
        ta.value = newValue;

        const cursor = (before + prefix + text + suffix).length;
        ta.setSelectionRange(cursor, cursor);
        return newValue;
    }
};
