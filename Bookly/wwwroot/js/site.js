let degree = 1800;
let clicks = 0;

$(document).ready(function () {
    $('#spin').click(function () {
        clicks++;
        let newDegree = degree * clicks;
        let extraDegree = Math.floor(Math.random() * 360);
        let totalDegree = newDegree + extraDegree;

        $('#inner-wheel').css({
            'transition': 'transform 4s ease-out',
            'transform': 'rotate(' + totalDegree + 'deg)'
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
                    var modal = new bootstrap.Modal(document.getElementById('bookModal'));
                    modal.show();
                })
                .catch(error => {
                    console.error('Fetch error:', error);
                    alert('Failed to spin the wheel. Please try again.');
                });
        }, 4000);
    });
});
