import { Link } from 'react-router-dom';
import React from 'react';

// Props & children
// type Props = { children: React.ReactNode } — de component verwacht kinderen (content) die in <main> worden gerenderd.
// Gebruik: <Layout>...jouw pagina...</Layout> (zoals in App.tsx).
type Props = { children: React.ReactNode };

export default function Layout({ children }: Props) {
    return (
        <div className="min-h-screen bg-zinc-900 text-zinc-100">
            <header className="bg-gradient-to-b from-zinc-900/70 to-zinc-900/90 border-b border-zinc-800">
                <div className="max-w-5xl mx-auto px-4 sm:px-6 lg:px-8">
                    <div className="flex items-center justify-between h-16">
                        <h1 className="text-lg sm:text-2xl font-extrabold tracking-tight">
                            OSRS Tool
                        </h1>
                        {/* TODO: nav to own file */}
                        <nav className="flex gap-6">
                            <Link
                                className="text-green-300 hover:text-green-200"
                                to="/">
                                Home
                            </Link>
                            <Link
                                className="text-green-300 hover:text-green-200"
                                to="/about">
                                About
                            </Link>
                        </nav>
                    </div>
                </div>
            </header>

            <main className="max-w-5xl mx-auto px-4 sm:px-6 lg:px-8 py-10">
                {children}
            </main>
        </div>
    );
}
