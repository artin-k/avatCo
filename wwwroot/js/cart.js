// Cart functionality for shop page
class CartManager {
    constructor() {
        this.init();
    }

    init() {
        this.updateCartCount();
    }

    // Add to cart function
    async addToCart(productId, quantity = 1) {
        const button = document.querySelector(`[data-product-id="${productId}"]`);
        const originalText = button.innerHTML;
        
        // Show loading state
        button.innerHTML = '...';
        button.disabled = true;
        button.classList.add('loading');

        try {
            const response = await fetch('/Cart/AddToCart', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]')?.value
                },
                body: JSON.stringify({
                    productId: productId,
                    quantity: quantity
                })
            });

            const result = await response.json();

            if (result.success) {
                this.showToast('✅ محصول به سبد خرید اضافه شد', 'success');
                this.updateCartCount();
            } else {
                this.showToast(`❌ ${result.message}`, 'error');
            }
        } catch (error) {
            this.showToast('❌ خطا در ارتباط با سرور', 'error');
        } finally {
            // Restore button
            button.innerHTML = originalText;
            button.disabled = false;
            button.classList.remove('loading');
        }
    }

    // Update cart count in header
    async updateCartCount() {
        try {
            const response = await fetch('/Cart/GetCartCount');
            const result = await response.json();
            
            const cartBadge = document.querySelector('.cart-count-badge');
            if (cartBadge) {
                cartBadge.textContent = result.count;
                cartBadge.style.display = result.count > 0 ? 'flex' : 'none';
            }
        } catch (error) {
            console.error('Error updating cart count:', error);
        }
    }

    // Show toast notification
    showToast(message, type = 'info') {
        // Remove existing toasts
        document.querySelectorAll('.cart-toast').forEach(toast => toast.remove());

        const toast = document.createElement('div');
        toast.className = `cart-toast cart-toast-${type}`;
        toast.innerHTML = message;

        document.body.appendChild(toast);

        setTimeout(() => toast.classList.add('show'), 100);
        setTimeout(() => {
            toast.classList.remove('show');
            setTimeout(() => toast.remove(), 300);
        }, 3000);
    }
}

// Initialize when page loads
document.addEventListener('DOMContentLoaded', () => {
    window.cartManager = new CartManager();
});

// Global function for buttons
window.addToCart = function(productId, quantity) {
    window.cartManager.addToCart(productId, quantity);
};