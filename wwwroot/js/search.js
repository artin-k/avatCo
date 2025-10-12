// wwwroot/js/search.js
class SearchHandler {
    constructor() {
        this.searchInput = document.querySelector('.search-input');
        this.searchForm = document.querySelector('#searchForm');
        this.init();
    }

    init() {
        this.setupAutoComplete();
    }

    setupAutoComplete() {
        if (!this.searchInput) return;

        this.searchInput.addEventListener('input', this.debounce(() => {
            this.fetchSuggestions();
        }, 300));
    }

    async fetchSuggestions() {
        const term = this.searchInput.value.trim();
        
        if (term.length < 2) {
            return;
        }

        try {
            const response = await fetch(`/Shop/QuickSearch?term=${encodeURIComponent(term)}`);
            const suggestions = await response.json();
            
            // You can implement suggestion display here
            console.log('Search suggestions:', suggestions);
        } catch (error) {
            console.error('Error fetching suggestions:', error);
        }
    }

    debounce(func, wait) {
        let timeout;
        return function executedFunction(...args) {
            const later = () => {
                clearTimeout(timeout);
                func(...args);
            };
            clearTimeout(timeout);
            timeout = setTimeout(later, wait);
        };
    }
}

// Initialize when DOM is loaded
document.addEventListener('DOMContentLoaded', () => {
    new SearchHandler();
});