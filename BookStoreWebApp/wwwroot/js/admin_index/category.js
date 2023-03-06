function updateCategory(newName, method = 'post') {

    // The rest of this code assumes you are not using a library.
    // It can be made less verbose if you use one.
    const form = document.createElement('form');
    form.method = method;
    form.action = 'Update';

    const hiddenField = document.createElement('input');
    hiddenField.type = 'hidden';
    hiddenField.name = 'Name';
    hiddenField.value = newName;
    form.appendChild(hiddenField);
    document.body.appendChild(form);
    form.submit();
}