/* BEGIN EXTERNAL SOURCE */
function name0() {
closeDeleteNewsModal()
}
/* END EXTERNAL SOURCE */
/* BEGIN EXTERNAL SOURCE */
function name1() {
closeDeleteNewsModal()
}
/* END EXTERNAL SOURCE */
/* BEGIN EXTERNAL SOURCE */


    function showDeleteNewsModal(title, id) {
        document.getElementById('modalNewsTitle').textContent = title;
        document.getElementById('modalNewsId').textContent = id;
        document.getElementById('modalNewsAvatar').textContent = title.charAt(0).toUpperCase();
        document.getElementById('hiddenNewsIdToDelete').value = id;

        const modal = document.getElementById('deleteNewsModal');
        document.getElementById('deleteNewsModal').classList.add('active');
        document.body.style.overflow = 'hidden';
    }

    function closeDeleteNewsModal() {
        const modal = document.getElementById('deleteNewsModal');
        document.getElementById('deleteNewsModal').classList.remove('active');
        document.body.style.overflow = 'auto';
    }

    document.getElementById('deleteNewsModal').addEventListener('click', function (e) {
        if (e.target === this) {
            closeDeleteNewsModal();
        }
    });



/* END EXTERNAL SOURCE */
