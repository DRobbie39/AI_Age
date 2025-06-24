document.addEventListener('DOMContentLoaded', () => {
    const chatToggleBtn = document.getElementById('chat-toggle');
    const chatContainer = document.getElementById('chat-container');
    const closeChatBtn = document.getElementById('close-chat');
    const chatBody = document.getElementById('chat-body');
    const chatInput = document.getElementById('chat-input');
    const sendBtn = document.getElementById('send-btn');
    const micBtn = document.getElementById('mic-btn');

    const token = localStorage.getItem('jwtToken');
    if (!token) {
        const chatWidget = document.querySelector('.chat-widget-container');
        if (chatWidget) {
            chatWidget.style.display = 'none';
        }
        return;
    }

    const SpeechRecognition = window.SpeechRecognition || window.webkitSpeechRecognition;
    const recognition = SpeechRecognition ? new SpeechRecognition() : null;
    let isListening = false;

    if (recognition) {
        recognition.continuous = false;
        recognition.lang = 'vi-VN';
        recognition.interimResults = false;

        recognition.onresult = (event) => {
            const transcript = event.results[0][0].transcript;
            chatInput.value = transcript;
            micBtn.classList.remove('listening');
            micBtn.innerHTML = '<i class="fas fa-microphone"></i>';
            isListening = false;
            handleSendMessage();
        };

        recognition.onerror = (event) => {
            console.error('Speech recognition error:', event.error);
            toastr.error('Con không nghe rõ, ông/bà vui lòng thử lại ạ.');
            micBtn.classList.remove('listening');
            micBtn.innerHTML = '<i class="fas fa-microphone"></i>';
            isListening = false;
        };

        recognition.onend = () => {
            if (isListening) {
                micBtn.classList.remove('listening');
                micBtn.innerHTML = '<i class="fas fa-microphone"></i>';
                isListening = false;
            }
        };
    } else {
        if (micBtn) micBtn.style.display = 'none';
    }

    const toggleChat = () => chatContainer.classList.toggle('open');

    const addMessageToChat = (text, sender) => {
        const messageDiv = document.createElement('div');
        messageDiv.className = `chat-message ${sender}`;
        messageDiv.innerHTML = `<p>${text}</p>`;
        chatBody.appendChild(messageDiv);
        chatBody.scrollTop = chatBody.scrollHeight;
    };

    const showTypingIndicator = () => {
        const typingDiv = document.createElement('div');
        typingDiv.className = 'chat-message bot typing-indicator';
        typingDiv.innerHTML = `<p><span>.</span><span>.</span><span>.</span></p>`;
        chatBody.appendChild(typingDiv);
        chatBody.scrollTop = chatBody.scrollHeight;
    };

    const removeTypingIndicator = () => {
        const indicator = document.querySelector('.typing-indicator');
        if (indicator) {
            indicator.remove();
        }
    };

    const speak = (text) => {
        if ('speechSynthesis' in window) {
            window.speechSynthesis.cancel();
            const utterance = new SpeechSynthesisUtterance(text);
            utterance.lang = 'vi-VN';
            utterance.rate = 0.9;
            window.speechSynthesis.speak(utterance);
        }
    };

    const handleSendMessage = async () => {
        const question = chatInput.value.trim();
        if (!question) return;

        addMessageToChat(question, 'user');
        chatInput.value = '';
        showTypingIndicator();

        try {
            const response = await fetch('https://localhost:7022/api/Chat/ask', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                },
                body: JSON.stringify({ Question: question })
            });

            removeTypingIndicator();

            if (!response.ok) {
                throw new Error('Network response was not ok.');
            }

            const data = await response.json();
            const botResponse = data.message || 'Xin lỗi, đã có lỗi xảy ra.';
            addMessageToChat(botResponse, 'bot');
            speak(botResponse);

        } catch (error) {
            console.error('Fetch error:', error);
            removeTypingIndicator();
            const errorMessage = 'Con xin lỗi, đã có lỗi xảy ra. Ông/Bà vui lòng thử lại sau ạ.';
            addMessageToChat(errorMessage, 'bot');
            speak(errorMessage);
        }
    };

    const handleMicClick = () => {
        if (!recognition) return;

        if (isListening) {
            recognition.stop();
        } else {
            recognition.start();
            isListening = true;
            micBtn.classList.add('listening');
            micBtn.innerHTML = '<i class="fas fa-stop-circle"></i>';
            toastr.info('Con đang lắng nghe đây ạ...');
        }
    };

    chatToggleBtn.addEventListener('click', toggleChat);
    closeChatBtn.addEventListener('click', toggleChat);
    sendBtn.addEventListener('click', handleSendMessage);
    if (micBtn) micBtn.addEventListener('click', handleMicClick);
    chatInput.addEventListener('keypress', (e) => {
        if (e.key === 'Enter') {
            handleSendMessage();
        }
    });
});
