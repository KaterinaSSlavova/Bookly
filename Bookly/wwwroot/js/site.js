$(document).ready(function () {
    $('#spin').click(function () {
        const $spinBtn = $(this);

        if ($spinBtn.prop('disabled')) {
            return;
        }
        $spinBtn.prop('disabled', true);

        let baseRotation = 1800; 
        let extraRotation = Math.floor(Math.random() * 360);
        let totalRotation = baseRotation + extraRotation;

        $('#inner-wheel').css({
            'transition': 'transform 4s ease-out',
            'transform': 'rotate(' + totalRotation + 'deg)'
        });

        setTimeout(function () {
            const token = $('input[name="__RequestVerificationToken"]').val();

            fetch(spinUrl, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded'
                },
                body: `__RequestVerificationToken=${encodeURIComponent(token)}`
            })
                .then(response => {
                    if (!response.ok) throw new Error(`Server error: ${response.status}`);
                    return response.json();
                })
                .then(data => {
                    $('#bookModalLabel').text(data.title);
                    $('input[name="Id"]').val(data.id);
                    $('input[name="Title"]').val(data.title);
                    $('input[name="Picture"]').val(data.picture);
                    $('#bookImage').attr('src', data.picture);
                    $('input[name="Author"]').val(data.author);
                    $('#bookAuthor').text(data.author);
                    $('input[name="Description"]').val(data.description);
                    $('#bookDescription').text(data.description);
                    $('input[name="Genre"]').val(data.genre);
                    $('#bookGenre').text(data.genre);
                    $('input[name="ISBN"]').val(data.isbn);
                    $('#bookISBN').text(data.isbn);

                    const modalElement = document.getElementById('bookModal');

                    if ($(modalElement).hasClass('show')) {
                        $(modalElement).modal('hide');
                    }
                    const modal = bootstrap.Modal.getInstance(modalElement) || new bootstrap.Modal(modalElement);

                    $(modalElement).off('hidden.bs.modal'); 
                    $(modalElement).on('hidden.bs.modal', function () {
                        $spinBtn.prop('disabled', false);

                        $('.modal-backdrop').remove();
                    });

                    modal.show();
                })
                .catch(error => {
                    console.error('Fetch error:', error);
                    alert('Failed to spin the wheel. Please try again.');
                    $spinBtn.prop('disabled', false);

                    $('.modal-backdrop').remove();
                });
        }, 4000);
    });
});
