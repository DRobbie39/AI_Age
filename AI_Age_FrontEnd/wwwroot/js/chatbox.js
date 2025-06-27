document.addEventListener('DOMContentLoaded', () => {
    const chatToggleBtn = document.getElementById('chat-toggle');
    const chatContainer = document.getElementById('chat-container');
    const closeChatBtn = document.getElementById('close-chat');
    const chatBody = document.getElementById('chat-body');
    const chatInput = document.getElementById('chat-input');
    const sendBtn = document.getElementById('send-btn');
    const micBtn = document.getElementById('mic-btn');

    // Ảnh đại diện
    const userAvatarUrl = '/img/profile.png';
    const botAvatarUrl = '/img/chatbox.jpg';

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
        const avatarUrl = sender === 'user' ? userAvatarUrl : botAvatarUrl;
        const messageDiv = document.createElement('div');
        messageDiv.className = `chat-message ${sender}`;
        messageDiv.innerHTML = `
            <img src="${avatarUrl}" alt="${sender} Avatar" class="chat-avatar">
            <p>${text}</p>
        `;
        chatBody.appendChild(messageDiv);
        chatBody.scrollTop = chatBody.scrollHeight;
    };

    const showTypingIndicator = () => {
        const typingDiv = document.createElement('div');
        typingDiv.className = 'chat-message bot typing-indicator';
        typingDiv.innerHTML = `
            <img src="${botAvatarUrl}" alt="Bot Avatar" class="chat-avatar">
            <p><span>.</span><span>.</span><span>.</span></p>
        `;
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

    const loadChatHistory = async () => {
        try {
            const response = await fetch('https://localhost:7022/api/Chat/history', {
                method: 'GET',
                headers: {
                    'Authorization': `Bearer ${token}`
                }
            });

            if (!response.ok) {
                throw new Error('Không thể tải lịch sử trò chuyện.');
            }

            const history = await response.json();
            chatBody.innerHTML = '';

            if (history.length === 0) {
                addMessageToChat('Xin chào! Ông/Bà có cần con giúp gì không ạ? Hãy hỏi con bất cứ điều gì nhé!', 'bot');
            } else {
                history.forEach(item => {
                    addMessageToChat(item.question, 'user');
                    addMessageToChat(item.response, 'bot');
                });
            }
        } catch (error) {
            console.error('Lỗi tải lịch sử chat:', error);
            addMessageToChat('Xin chào! Ông/Bà có cần con giúp gì không ạ? Hãy hỏi con bất cứ điều gì nhé!', 'bot');
        }
    };

    loadChatHistory();

    const displaySuggestions = (suggestions) => {
        const suggestionsContainer = document.createElement('div');
        suggestionsContainer.className = 'chat-suggestions';

        let suggestionHtml = '<p>Đây là một số nội dung con tìm được ạ:</p><ul>';

        suggestions.forEach(item => {
            // Icon dựa trên loại nội dung
            const icon = item.type === 'Article' ? 'fas fa-newspaper' : 'fas fa-video';
            suggestionHtml += `
                <li>
                    <a href="${item.url}" target="_blank">
                        <i class="${icon}"></i> ${item.title}
                    </a>
                </li>
            `;
        });

        suggestionHtml += '</ul>';
        suggestionsContainer.innerHTML = suggestionHtml;
        chatBody.appendChild(suggestionsContainer);
        chatBody.scrollTop = chatBody.scrollHeight;
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
            const botResponseText = data.response || 'Xin lỗi, đã có lỗi xảy ra.';
            addMessageToChat(botResponseText, 'bot');
            speak(botResponseText);

            // Kiểm tra và hiển thị các gợi ý nếu có
            if (data.suggestions && data.suggestions.length > 0) {
                displaySuggestions(data.suggestions);
            }

            setTimeout(() => {
                switch (data.action) {
                    case 'navigate':
                        if (data.url) {
                            window.location.href = data.url;
                        }
                        break;

                    case 'open_external':
                        if (data.url) {
                            window.open(data.url, '_blank');
                        }
                        break;

                    case 'talk':
                    case 'suggest_content':
                    default:
                        break;
                }
            }, 1500);

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
